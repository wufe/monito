using System;
using Monito.Persistence.Model.Interface;

namespace Monito.Persistence.Model {
	public class LinkPersistenceModel : IPrimaryKeyEntity, ITimestampTrackedEntity, IUUIDTrackedEntity {
		public int ID { get; set; }
		public string URL { get; set; }
		public LinkStatus Status { get; set; }
		public string Output { get; set; }
		public int StatusCode { get; set; }
		public string AdditionalData { get; set; }
		public Guid UUID { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int? RedirectsFromLinkId { get; set; }
		public LinkPersistenceModel RedirectsFrom { get; set; }
		public LinkPersistenceModel RedirectsTo { get; set; }
		public int RequestID { get; set; }
		public RequestPersistenceModel Request { get; set; }
	}

	public enum LinkStatus : byte {
		Idle = 0,
		Acknowledged = 1,
		InProgress = 2,
		Done = 3
	}
}