using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;

namespace GrpcClient1
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			// The port number(5001) must match the port of the gRPC server.
			using var channel = GrpcChannel.ForAddress("https://localhost:5001");

			var client = new Greeter.GreeterClient(channel);
			var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });
			Console.WriteLine("Greeting: " + reply.Message);
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();

			var client2 = new Test.TestClient(channel);
			using var stream = client2.StreamingTestWays();
			var response = Task.Run(async () =>
			{
				await foreach(var rm in stream.ResponseStream.ReadAllAsync())
						Console.WriteLine(rm.Message);
			});
			Console.WriteLine("enter message to stream to server");
			while (true)
			{
				var text = Console.ReadLine();
				if (string.IsNullOrEmpty(text)) break;

				await stream.RequestStream.WriteAsync(new ClientRequest()
				{
					Name =" this client"
				});
			}

			Console.WriteLine("Disconnecting.........");
			await stream.RequestStream.CompleteAsync();
			await response;
		}

		// Additional configuration is required to successfully run gRPC on macOS.
		// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
