using Microsoft.AspNetCore.Http;

namespace Base.Models
{
	public class MultipartFormDataModel
	{
		public string Name { get; set; }
		public string IdNo { get; set; }
		public IFormFileCollection Files { get; set; }
	}
}