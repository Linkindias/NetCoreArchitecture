using System;
using System.Threading.Tasks;
using BLL;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcService1
{
	public class TestService : Test.TestBase
	{
		private readonly ILogger<TestService> _logger;
		private MemberService memberService;

		public TestService(MemberService memberService, ILogger<TestService> logger)
		{
			this.memberService = memberService;
			_logger = logger;
		}

		public override async Task StreamingTestWays(IAsyncStreamReader<ClientRequest> request, IServerStreamWriter<ServerResponse> response, ServerCallContext context)
		{
			if (request != null)
			{
				if (!await request.MoveNext()) return;
			}

			try
			{
				if (!string.IsNullOrEmpty(request.Current.Name))
				{
					Console.WriteLine($"client: {request.Current.Name}");
				}

				await response.WriteAsync(new ServerResponse()
				{
					Message = "server response"
				});
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	}
}