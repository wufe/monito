using System;
using Monito.Database.Entities.Interface;

namespace Monito.Database.Entities {
	public class Link : IPrimaryKeyEntity, ITimestampTrackedEntity {
		public int ID { get; set; }
		public string URL { get; set; }
		public string OriginalURL { get; set; }
		public int StatusCode { get; set; }
		public string AdditionalData { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int? RedirectsFromLinkId { get; set; }
		public virtual Link RedirectsFrom { get; set; }
		public int RequestID { get; set; }
		public virtual Request Request { get; set; }
	}
}