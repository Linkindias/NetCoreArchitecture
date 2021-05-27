using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BLL;
using DAL;
using DAL.Repo;
using DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WebApplication1
{
    public class Startup
    {
        private IConfiguration config { get; }
        private IMemoryCache memoryCache { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews().AddSessionStateTempDataProvider();
            services.AddControllers().AddNewtonsoftJson();
            services.AddControllers().AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.WriteIndented = true;
                option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                option.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddMemoryCache();

            //DbContext
            services.AddDbContext<OneDbContext>(options => options.UseSqlServer(Configuration.GetSection("OneContext").Value), ServiceLifetime.Transient);
            services.AddDbContext<TwoDbContext>(options => options.UseSqlServer(Configuration.GetSection("TwoContext").Value), ServiceLifetime.Transient);

            services.AddTransient<OneAccountRepository>();
            services.AddTransient<TwoAccountRepository>();
            services.AddTransient<UserEventLogRepository>();
            services.AddTransient<ExceptionLogRepository>();
            
            services.AddScoped<MemberService>();
            services.AddScoped<OperateLogService>();
            services.AddScoped<ExceptionLogService>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IMemoryCache, MemoryCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
