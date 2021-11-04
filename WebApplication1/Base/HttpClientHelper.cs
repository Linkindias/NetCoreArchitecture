using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;
using Base.Models;

namespace Base
{
	public	class HttpClientHelper
	{
		private readonly IHttpClientFactory _clientFactory;

		public HttpClientHelper(IHttpClientFactory clientFactory)
		{
			_clientFactory = clientFactory;
		}

		public async Task<T> SendGet<T>(string url)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, url);
			var client = _clientFactory.CreateClient();
			var response = await client.SendAsync(request);

			if (!response.IsSuccessStatusCode) throw new HttpResponseException(response);

			return await response.Content.ReadAsAsync<T>();
		}

		public async Task<T> SendPostForJson<T>(string url, object parameters)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, url);
			request.Content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json");
			var client = _clientFactory.CreateClient();
			var response = await client.SendAsync(request);

			if (!response.IsSuccessStatusCode) throw new HttpResponseException(response);

			return await response.Content.ReadAsAsync<T>();
		}

		public async Task<T> SendPostForForm<T>(string url, List<KeyValuePair<string, string>> postData)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, url);
			request.Content = new FormUrlEncodedContent(postData) {
				Headers =
				{
					ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded")
					{
						CharSet = "UTF-8"
					}
				}
			};
			var client = _clientFactory.CreateClient();
			var response = await client.SendAsync(request);

			if (!response.IsSuccessStatusCode) throw new HttpResponseException(response);

			return await response.Content.ReadAsAsync<T>();
		}

		public async Task<T> SendPostForDocument<T>(string url, Dictionary<string, string> dictionarys, List<StreamModel> streams)
		{
			MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
			multipartFormDataContent.Headers.ContentType.MediaType = "multipart/form-data";
			foreach (var content in dictionarys)
				multipartFormDataContent.Add(new StringContent(content.Value), content.Key);

			foreach (StreamModel stream in streams)
				multipartFormDataContent.Add(new StreamContent(stream.Stream), stream.Name, stream.FileName);

			var request = new HttpRequestMessage(HttpMethod.Post, url);
			request.Content = multipartFormDataContent;

			var client = _clientFactory.CreateClient();
			var response = await client.SendAsync(request);

			if (!response.IsSuccessStatusCode) throw new HttpResponseException(response);

			return await response.Content.ReadAsAsync<T>();
		}
	}
}
