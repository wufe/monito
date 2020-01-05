using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Monito.Database.Entities;

namespace Monito.ValueObjects.Output {
	public class RetrieveJobOutputModel {
		public int ID { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public RequestType Type { get; set; }

		public RequestOptions Options { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public RequestStatus Status { get; set; }
		
		public ICollection<RetrieveLinkOutputModel> Links { get; set; }
		
		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}
}