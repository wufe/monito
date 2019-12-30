using System;
using System.Text.Json.Serialization;
using Monito.Database.Entities;

namespace Monito.ValueObjects {
	public class RetrieveJobOutputModel {

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public RequestType Type { get; set; }

		public RequestOptions Options { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public RequestStatus Status { get; set; }
		
		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}
}