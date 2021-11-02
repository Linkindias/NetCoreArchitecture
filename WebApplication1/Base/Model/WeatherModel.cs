using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Base.Models
{
	public class WeatherModel
	{
		public bool success { get; set; }
		public HttpStatusCode code { get; set; }
		public string msg { get; set; }
		public ResultModel result { get; set; }
		public RecordsModel records { get; set; }
	}
}
