using System;
using System.Text.Json.Serialization;
using Monito.Database.Entities;

namespace Monito.ValueObjects.Output {
	public class RetrieveJobStatusOutputModel {

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public RequestStatus Status { get; set; }

		public DateTime UpdatedAt { get; set; }
	}
}