using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApplication1
{
	public class ExceptionMiddleware
	{
		//UseMiddleware startup
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				bool isApi = Regex.IsMatch(context.Request.Path.Value, "^/api/", RegexOptions.IgnoreCase);
				if (isApi)
				{
					if (context.Request.Method == "POST")
					{
						context.Request.EnableBuffering();
						string requestContent;
						using (var reader = new StreamReader(context.Request.Body))
						{
							requestContent = await reader.ReadToEndAsync();
						}
						context.Items.Add("body", requestContent);
					}
				}
				await _next(context);
			}
			catch (Exception ex)
			{
				await context.Response.WriteAsync($"{GetType().Name} catch excetion. Message:{ex.Message}");
			}
		}
	}
}