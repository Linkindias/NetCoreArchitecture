using BLL;
using BLL.MapperModel;
using DAL;
using DAL.Repo;
using DAL.Repository;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using AspNetCoreRateLimit;
using FluentValidation;
using FluentValidation.AspNetCore;
using WebApplication1.Filter;
using WebApplication1.Models;
using WebApplication1.Validator;
using System.Reflection;

namespace WebApplication1
{
	public class Startup
    {
        private IMemoryCache memoryCache { get; set; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
	        services.AddMvc().AddFluentValidation(fv =>
	        {
		        fv.ImplicitlyValidateChildProperties = true;
		        fv.ImplicitlyValidateRootCollectionElements = true;
		        fv.DisableDataAnnotationsValidation = true;
                fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
	        });

	        services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews(
                options => options.Filters.Add(new ExceptionAttribute())
                ).AddSessionStateTempDataProvider();
            services.AddControllers().AddNewtonsoftJson();
            services.AddControllers().AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.WriteIndented = true;
                option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                option.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddOptions();

            services.AddMemoryCache();

            services.AddAutoMapper(typeof(MappingAccount));

            // 從 appsettings.json 讀取 IpRateLimiting 設定 
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            // 從 appsettings.json 讀取 Ip Rule 設定
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            // 注入 counter and IP Rules 
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            //DbContext
            services.AddDbContext<OneDbContext>(options => options.UseSqlServer(Configuration.GetSection("OneContext").Value,builder => builder.EnableRetryOnFailure()), ServiceLifetime.Transient);
            services.AddDbContext<TwoDbContext>(options => options.UseSqlServer(Configuration.GetSection("TwoContext").Value,builder => builder.EnableRetryOnFailure()), ServiceLifetime.Transient);

            services.AddTransient<OneAccountRepository>();
            services.AddTransient<TwoAccountRepository>();
            services.AddTransient<UserEventLogRepository>();
            services.AddTransient<ExceptionLogRepository>();
            
            services.AddScoped<MemberService>();
            services.AddScoped<OperateLogService>();
            services.AddScoped<ExceptionLogService>();

            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IMemoryCache, MemoryCache>();

            // the clientId/clientIp resolvers use it.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Rate Limit configuration 設定
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiforgery)
        {
	        //app.UseMiddleware<ExceptionMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseIpRateLimiting();

            // for body streamReader
            app.Use((context, next) =>
            {
                context.Request.EnableBuffering();
                return next();
            });

            //exception for webapi
            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                ExceptionHandler = async (baseContext) =>
                {
                    bool isApi = Regex.IsMatch(baseContext.Request.Path.Value, "^/api/", RegexOptions.IgnoreCase);
                    if (isApi)
                    {
                        var exception = baseContext.Features.Get<IExceptionHandlerFeature>();
                        baseContext.Response.ContentType = "application/json";
                        baseContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        string parameters = string.Empty;

                        if (baseContext.Request.Method == "GET")
                            parameters = baseContext.Request.QueryString.Value;
                        else
                        {
                            var bodyStream = new StreamReader(baseContext.Request.Body);
                            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                            parameters = bodyStream.ReadToEnd();
                        }

                        await baseContext.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            StatusCode = baseContext.Response.StatusCode,
                            Message = exception.Error.Message,
                            Path = baseContext.Request.Path.Value,
                            Method = baseContext.Request.Method,
                            Parameters = parameters,
                            IP = baseContext.Connection.RemoteIpAddress.ToString()
                        }));
                        return;
                    }
                    baseContext.Response.Redirect("/error");
                }
            });

			//csrf for webapi

			app.Use(next => context =>
			{
				string path = context.Request.Path.Value;

				if (string.Equals(path, "/", StringComparison.OrdinalIgnoreCase) || string.Equals(path, "/api/Security/LogIn", StringComparison.OrdinalIgnoreCase))
				{
					var tokens = antiforgery.GetAndStoreTokens(context);
					context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions() { HttpOnly = false });
				}

				return next(context);
			});

			var forwardingOptions = new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            forwardingOptions.KnownNetworks.Clear(); // Loopback by default, this should be temporary
            forwardingOptions.KnownProxies.Clear(); // Update to include

            app.UseForwardedHeaders(forwardingOptions);

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
