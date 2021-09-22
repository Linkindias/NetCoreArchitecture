using BLL;
using DAL;
using DAL.Repo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using BLL.MapperModel;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace GrpcService1
{
	public class Startup
	{
		private IMemoryCache memoryCache { get; set; }
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddGrpc(
				options =>
				{
					options.Interceptors.Add<LogInterceptor>();
				});

			services.AddAutoMapper(typeof(MappingAccount));

			//DbContext
			services.AddDbContext<OneDbContext>(options => options.UseSqlServer(Configuration.GetSection("OneContext").Value, builder => builder.EnableRetryOnFailure()), ServiceLifetime.Transient);
			services.AddDbContext<TwoDbContext>(options => options.UseSqlServer(Configuration.GetSection("TwoContext").Value, builder => builder.EnableRetryOnFailure()), ServiceLifetime.Transient);

			services.AddTransient<MemberService>();
			services.AddTransient<OneAccountRepository>();
			services.AddTransient<TwoAccountRepository>();

			
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGrpcService<TestService>();
				endpoints.MapGrpcService<GreeterService>();

				endpoints.MapGet("/", async context =>
				{
					await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
				});
			});
		}
	}
}
