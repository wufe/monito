using System;

namespace Monito.ValueObjects.Output {
	// TODO: Delete
	public class RetrieveLinkOutputModel : RetrieveBriefLinkOutputModel {
		public Guid UUID { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int RequestID { get; set; }
	}
}