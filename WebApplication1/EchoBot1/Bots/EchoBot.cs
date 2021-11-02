// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.14.0

using System;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Base;
using Base.Models;
using Newtonsoft.Json;

namespace EchoBot1.Bots
{
	public class EchoBot : ActivityHandler
	{
		private readonly HttpClientHelper _httpClientHelper;

		public EchoBot(HttpClientHelper httpClientHelper)
		{
			_httpClientHelper = httpClientHelper;
		}

		List<string> cityName = new List<string> { "基隆市", "臺北市", "新北市", "桃園市", "新竹市", "新竹縣", "苗栗縣", "臺中市", "南投縣", "雲林縣", 
													"彰化縣", "嘉義市", "嘉義縣", "臺南市", "高雄市", "屏東縣", "宜蘭縣", "花蓮縣", "臺東縣", "澎湖縣", "金門縣", "連江縣" };
		protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
		{
			var replyText = turnContext.Activity.Text;

			if (!cityName.Contains(replyText.Replace("台", "臺")) || string.IsNullOrEmpty(replyText))
				await turnContext.SendActivityAsync(MessageFactory.Text($"Please Input Corrent City:{replyText}"), cancellationToken);

			else
			{
				var response = await _httpClientHelper.SendGet($"http://localhost/webapplication/api/Weather?locationName={replyText.Replace("台", "臺")}");

				try
				{
					if (response.IsSuccessStatusCode)
					{
						var stream = await response.Content.ReadAsStreamAsync();

						using (var streamReader = new StreamReader(stream))
						{
							using (var jsonTextReader = new JsonTextReader(streamReader))
							{
								var jsonSerializer = new JsonSerializer();
								var weather = jsonSerializer.Deserialize<WeatherModel>(jsonTextReader);

								if (weather.success && weather.records.location.Length > 0)
								{
									string result = $"City : {weather.records.location[0].locationName}\r\n";

									var status = weather.records.location[0].weatherElement.FirstOrDefault(o => o.elementName == "Wx");
									var percentage = weather.records.location[0].weatherElement.FirstOrDefault(o => o.elementName == "PoP");
									var min = weather.records.location[0].weatherElement.FirstOrDefault(o => o.elementName == "MinT");
									var max = weather.records.location[0].weatherElement.FirstOrDefault(o => o.elementName == "MaxT");
									var ci = weather.records.location[0].weatherElement.FirstOrDefault(o => o.elementName == "CI");
								
									if (status.time.Length > 0 && percentage.time.Length > 0 && max.time.Length > 0 && min.time.Length > 0 && ci.time.Length > 0)
									{
										result += $"{status.time[0].startTime.ToString("yyyy/MM/dd HH:mm")} ~ {status.time[0].endTime.ToString("HH:mm")} \r\n";
										result += $"{status.time[0].parameter.parameterName}, 下雨率 :{percentage.time[0].parameter.parameterName}% ,{min.time[0].parameter.parameterName}~{max.time[0].parameter.parameterName} C {ci.time[0].parameter.parameterName}\r\n";
										result += $"{status.time[1].startTime.ToString("yyyy/MM/dd HH:mm")} ~ {status.time[1].endTime.ToString("HH:mm")} \r\n";
										result += $"{status.time[1].parameter.parameterName}, 下雨率 :{percentage.time[1].parameter.parameterName}% ,{min.time[1].parameter.parameterName}~{max.time[1].parameter.parameterName} C {ci.time[1].parameter.parameterName}\r\n";
										result += $"{status.time[2].startTime.ToString("yyyy/MM/dd HH:mm")} ~ {status.time[2].endTime.ToString("HH:mm")} \r\n";
										result += $"{status.time[2].parameter.parameterName}, 下雨率 :{percentage.time[2].parameter.parameterName}% ,{min.time[2].parameter.parameterName}~{max.time[2].parameter.parameterName} C {ci.time[2].parameter.parameterName}\r\n";
									}

									await turnContext.SendActivityAsync(MessageFactory.Text(result), cancellationToken);
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					await turnContext.SendActivityAsync(MessageFactory.Text($"Weather Query :{ex.Message}"), cancellationToken);
				}
			}
			//await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
			//if (string.Equals(turnContext.Activity.Text, "wait", System.StringComparison.InvariantCultureIgnoreCase))
			//{
			//	await turnContext.SendActivitiesAsync(
			//		new Activity[] {
			//			new Activity { Type = ActivityTypes.Typing },
			//			new Activity { Type = "delay", Value= 3000 },
			//			MessageFactory.Text("Finished typing", "Finished typing"),
			//		},
			//		cancellationToken);
			//}
			//else
			//{
			//	var replyText = $"Echo: {turnContext.Activity.Text}. Say 'wait' to watch me type.";
			//	await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
			//}
		}

		protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
		{
			var welcomeText = "Hello and welcome, you can query city weather !";
			foreach (var member in membersAdded)
			{
				if (member.Id != turnContext.Activity.Recipient.Id)
				{
					await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
				}
			}
		}

		//public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
		//{

		//	if (turnContext.Activity.Type == ActivityTypes.Message)
		//	{
		//		var usermsg = turnContext.Activity.Text;

		//		await turnContext.SendActivityAsync("bot response message");
		//	}
		//}
	}
}
