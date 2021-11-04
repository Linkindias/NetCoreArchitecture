using Microsoft.AspNetCore.Http;

namespace Base.Models
{
	public class MultipartFormDataModel
	{
		public string Test { get; set; }
		public IFormFileCollection Files { get; set; }
	}
}