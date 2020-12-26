using System;

namespace Monito.ValueObjects.Output {
	// TODO: Delete
	public class RetrieveBriefLinkOutputModel {
		public int ID { get; set; }
		public string URL { get; set; }
		public string Output { get; set; }
		public int StatusCode { get; set; }
		public int? RedirectsFromLinkId { get; set; }
		public int? RedirectsToLinkId { get; set; }
	}
}