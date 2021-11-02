using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Base;
using WebApplication1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.WebApi
{
	[Route("api/[controller]")]
	[ApiController]
	public class WeatherController : ControllerBase
	{
		private readonly IHttpClientFactory _clientFactory;
		private readonly HttpClientHelper _httpClientHelper;
		private string apiUrl = @"https://opendata.cwb.gov.tw/api/v1/rest/datastore/dataid?Authorization=apikey&format=JSON";
		private string weatherId = @"F-C0032-001";
		private string apikey = @"CWB-BC1EB9DB-7648-4419-8D75-870FAA8ADA2A";

		public WeatherController(IHttpClientFactory clientFactory, HttpClientHelper httpClientHelper)
		{
			_clientFactory = clientFactory;
			_httpClientHelper = httpClientHelper;
			apiUrl = apiUrl
							.Replace("apikey", apikey)
							.Replace("dataid", weatherId);
		}

		[HttpGet]
		public WeatherModel Get()
		{
			return Task<WeatherModel>.Run(async () => {

				WeatherModel wm = new WeatherModel() { success = false };
				var response = await _httpClientHelper.SendGet(apiUrl);

				try
				{
					if (response.IsSuccessStatusCode)
					{
						var result = await response.Content.ReadAsAsync<WeatherModel>();
						return result;
					}
					wm.code = response.StatusCode;
					return wm;
				}
				catch (Exception ex)
				{
					wm.msg = ex.Message;
					return wm;
				}
			}).Result;
		}

		// POST api/<WeatherController>
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/<WeatherController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<WeatherController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
