using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Models
{
	public class VideoInfo
	{
		public string type { get; set; }
		public string fileName { get; set; }
		public DateTime startDate { get; set; }
		public DateTime endDate { get; set; }
	}
}