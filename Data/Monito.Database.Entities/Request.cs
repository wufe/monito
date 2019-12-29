using System;
using System.Collections.Generic;
using Monito.Database.Entities.Interface;

namespace Monito.Database.Entities {
	public class Request : IPrimaryKeyEntity, ITimestampTrackedEntity {
		public int ID { get; set; }
		public RequestType Type { get; set; }
		public string Options { get; set; }
		public RequestStatus Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int UserID { get; set; }
		public virtual User User { get; set; }
		public virtual ICollection<File> Files { get; set; }
		public virtual ICollection<Link> Links { get; set; }
	}

	public enum RequestType : byte {
		Simple = 1,
		Batch  = 2
	}

	public enum RequestStatus : byte {
		Incomplete = 0,
		Ready      = 1,
		InProgress = 2,
		Done       = 3
	}
}