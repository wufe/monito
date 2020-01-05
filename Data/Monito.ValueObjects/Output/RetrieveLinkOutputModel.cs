using System;

namespace Monito.ValueObjects.Output {
	public class RetrieveLinkOutputModel {
		public int ID { get; set; }
		public string URL { get; set; }
		public string Output { get; set; }
		public int StatusCode { get; set; }
		public Guid UUID { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int? RedirectsFromLinkId { get; set; }
		public int? RedirectsToLinkId { get; set; }
		public int RequestID { get; set; }
	}
}