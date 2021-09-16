using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Logging;

namespace GrpcService1
{
	public class LogInterceptor : Interceptor
	{
		private readonly ILogger<LogInterceptor> logger;

		public LogInterceptor(ILogger<LogInterceptor> _logger)
		{
			logger = _logger;
		}

		public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
		{
			this.logger.LogWarning("Warn Unary logs.");

			return continuation(request, context);
		}

		public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
		{
			this.logger.LogWarning("Warn  DuplexStreaming logs.");
			await base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
		}
	}
}