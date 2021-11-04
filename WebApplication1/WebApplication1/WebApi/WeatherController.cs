﻿using Base;
using Base.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.WebApi
{
	//[EnableCors("Weather")]
	[Route("api/[controller]")]
	[ApiController]
	public class WeatherController : ControllerBase
	{
		private readonly HttpClientHelper _httpClientHelper;
		private string apiUrl = @"https://opendata.cwb.gov.tw/api/v1/rest/datastore/dataid?Authorization=apikey&format=JSON";
		private string weatherId = @"F-C0032-001";
		private string apikey = @"CWB-BC1EB9DB-7648-4419-8D75-870FAA8ADA2A";

		public WeatherController(HttpClientHelper httpClientHelper)
		{
			_httpClientHelper = httpClientHelper;
			apiUrl = apiUrl.Replace("apikey", apikey)
							.Replace("dataid", weatherId);
		}

		[HttpGet]
		public WeatherModel Get(string locationName)
		{
			if (!string.IsNullOrEmpty(locationName))
				apiUrl = $"{apiUrl}&locationName={locationName}";

			//add cache
			return Task<WeatherModel>.Run(async () =>
			{
				return await _httpClientHelper.SendGet<WeatherModel>(apiUrl);
			}).Result;
		}

		[Route("Test")]
		[HttpGet]
		public ResultModel Test(string Msg, int Count)
		{
			return Task<ResultModel>.Run(async () =>
			{
				//Dictionary<string, string> stringDictionary = new Dictionary<string, string>() { { "test", "ddd" } };
				//List<StreamModel> listStream = new List<StreamModel>();

				//string[] filePaths = Directory.GetFiles("D:\\Device", "*.png", SearchOption.AllDirectories);

				//foreach (string filePath in filePaths)
				//{
				//	string[] arFiles = filePath.Split('\\');
				//	string fileName = arFiles[^1];
				//	Stream fileStream = System.IO.File.OpenRead(filePath);
				//	listStream.Add(new StreamModel() { Stream = fileStream , Name = "files", FileName = fileName });
				//}
				//return await _httpClientHelper.SendPostForDocument<ResultModel>($"https://localhost:44349/api/Weather/Upload", stringDictionary, listStream);
				List<KeyValuePair<string,string>> lstKeyValues = new List<KeyValuePair<string,string>>()
				{
					new KeyValuePair<string, string>("name","joen"), new KeyValuePair<string, string>("value", "test")
				};

				return await _httpClientHelper.SendPostForForm<ResultModel>($"https://localhost:44349/api/Weather/Upload", lstKeyValues);
			}).Result; 
		}

		[Route("Upload")]
		[HttpPost]
		public ResultModel Post([FromForm] FormUrlencodedModel model)
		{
			if (model != null)
			{
				return new ResultModel() { success = true, msg = string.Empty };
			}

			return new ResultModel() { success = false, msg = "Error" };
		}
		//[Route("Upload")]
		//[HttpPost]
		//public ResultModel Post([FromForm] MultipartFormDataModel model) //multipart/form-data、application/x-www-form-urlencoded use FromForm, json use FromBody
		//{
		//	if (model != null)
		//	{
		//		return new ResultModel() { success = true, msg = string.Empty };
		//	}

		//	return new ResultModel() { success = false, msg = "Error" };
		//}

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
