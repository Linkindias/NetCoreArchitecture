using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApplication1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.WebApi
{
	[Route("api/[controller]")]
	[ApiController]
	public class VideoController : ControllerBase
	{
		[HttpGet]
		[Route("FileSchedule")]
		public IActionResult Get()
		{
			return new ObjectResult(new VideoViewModel()
			{
				count = 2,
				videos = new List<VideoInfo>()
				{
					new VideoInfo()
					{
						type ="img",
						fileName = "aspnetcore.png",
						startDate = DateTime.Today.AddHours(14).AddMinutes(30),
						endDate = DateTime.Today.AddHours(15),
					},
					new VideoInfo()
					{
						type ="video",
						fileName = "a.mp4",
						startDate = DateTime.Today.AddHours(12).AddMinutes(00),
						endDate = DateTime.Today.AddHours(12).AddMinutes(30),
					},
					new VideoInfo()
					{
						type ="video",
						fileName = "b.mp4",
						startDate = DateTime.Today.AddHours(12).AddMinutes(40),
						endDate = DateTime.Today.AddHours(13),
					},
					new VideoInfo()
					{
						type ="video",
						fileName = "c.mp4",
						startDate = DateTime.Today.AddHours(13).AddMinutes(30),
						endDate = DateTime.Today.AddHours(14),
					},
				}
			});
		}

		[HttpGet]
		[Route("DownLoadFile")]
		public IActionResult Download(string fileName)
		{
			var path = @$"d:\Device\{fileName}";
			var memory = new MemoryStream();
			using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 65536, FileOptions.Asynchronous | FileOptions.SequentialScan))
			{
				stream.CopyTo(memory);
			}
			memory.Position = 0;
			return File(memory, "application/octet-stream", Path.GetFileName(path), true);
		}

		// POST api/<VideoController>
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/<VideoController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<VideoController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
