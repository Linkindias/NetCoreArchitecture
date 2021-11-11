using System.Collections.Generic;
using System.IO;
using System.Net;

namespace WebApplication1.Models
{
	public class VideoViewModel
	{
		public int count { get; set; }
		public List<VideoInfo> videos { get; set; }	
	}
}