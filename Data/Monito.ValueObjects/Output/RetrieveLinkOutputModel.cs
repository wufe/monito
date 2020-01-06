using System;

namespace Monito.ValueObjects.Output {
	public class RetrieveLinkOutputModel : RetrieveBriefLinkOutputModel {
		public Guid UUID { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int RequestID { get; set; }
	}
}