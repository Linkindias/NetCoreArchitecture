using System;

namespace Base.Models
{
	public class RecordsModel
	{
		public string datasetDescription { get; set; }	
		public Location[] location { get; set; }
	}

	public class Location
	{
		public string locationName { get; set; }		
		public WeatherElement[] weatherElement { get; set; }

	}

	public class WeatherElement
	{
		public string elementName { get; set; }
		public Time[] time { get; set; }
	}

	public class Time
	{
		public DateTime startTime { get; set; }
		public DateTime endTime { get; set; }
		public Parameter parameter { get; set; }
	}

	public class Parameter
	{
		public string parameterName { get; set; }
		public string parameterValue { get; set; }
		public string parameterUnit { get; set; }
	}
}