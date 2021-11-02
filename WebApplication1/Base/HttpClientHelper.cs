using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
	public	class HttpClientHelper
	{
		private readonly IHttpClientFactory _clientFactory;
		private readonly HttpClient _client;

		public HttpClientHelper(IHttpClientFactory clientFactory)
		{
			_clientFactory = clientFactory;
		}

		public async Task<HttpResponseMessage> SendGet(string url)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, url);
			var client = _clientFactory.CreateClient();
			return await client.SendAsync(request);
		}
    }
}
