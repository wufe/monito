using System.Text.Json.Serialization;

namespace Monito.Web.Models.Input {
	public class SaveJobInputModel {
		public string Links { get; set; }
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public SaveJobHttpMethod Method { get; set; }
		public int Redirects { get; set; }
		public int Threads { get; set; }
		public int Timeout { get; set; }
		public string UserAgent { get; set; }
	}

	public enum SaveJobHttpMethod {
		GET,
		HEAD
	}
}