using System;
using Monito.Database.Entities.Interface;

namespace Monito.Database.Entities {
	public class Link : IPrimaryKeyEntity, ITimestampTrackedEntity, IUUIDTrackedEntity {
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
		public virtual Link RedirectsFrom { get; set; }
		public virtual Link RedirectsTo { get; set; }
		public int RequestID { get; set; }
		public virtual Request Request { get; set; }
	}

	public enum LinkStatus : byte {
		Idle = 0,
		InProgress = 1,
		Done = 2
	}
}