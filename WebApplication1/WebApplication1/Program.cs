using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using static System.Diagnostics.Debug;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
      //      var text = @"99 little bugs in the code, 99 bugs in the code. 
						//take one down , patch it around, 127 bugs in the code.
						//(Repeat until no more bugs)";
      //      var mostFrequentWord = text.Split(' ','.',',')
						//									.Where(i => i != "")
						//									.GroupBy(i => i)
						//									.OrderBy(i => i.Count())
						//									.Last();
      //      Assert(mostFrequentWord.Key == "bugs");


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
