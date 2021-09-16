﻿using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace GrpcClient1
{
	public class LogInterceptor : Interceptor
	{
		private readonly ILogger _logger;

		public LogInterceptor(ILogger logger)
		{
			_logger = logger;
		}

		public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
		{
			LogCall(context.Method);

			var call = continuation(request, context);

			return new AsyncUnaryCall<TResponse>(HandleResponse(call.ResponseAsync), call.ResponseHeadersAsync, call.GetStatus, call.GetTrailers, call.Dispose);
		}

		private async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> t)
		{
			try
			{
				var response = await t;
				_logger.LogDebug($"Response received: {response}");
				return response;
			}
			catch (RpcException ex)
			{
				_logger.LogError($"Call error: {ex.Message}");
				return default;
			}
		}

		private void LogCall<TRequest, TResponse>(Method<TRequest, TResponse> method) where TRequest : class where TResponse : class
		{
			_logger.LogDebug($"Starting call. Type: {method.Type}. Request: {typeof(TRequest)}. Response: {typeof(TResponse)}");
		}
	}
}