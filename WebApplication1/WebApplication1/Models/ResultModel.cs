namespace WebApplication1.Models
{
	public class ResultModel
	{
		public string resource_id { get; set; }
		public Fields[] fields { get; set; }
	}

	public class Fields
	{
		public string id { get; set; }
		public string type { get; set; }
	}
}