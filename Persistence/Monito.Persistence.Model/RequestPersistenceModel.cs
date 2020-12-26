using System;
using System.Collections.Generic;
using Monito.Persistence.Model.Interface;

namespace Monito.Persistence.Model {
	public class RequestPersistenceModel : IPrimaryKeyEntity, ITimestampTrackedEntity, IUUIDTrackedEntity {
		public int ID { get; set; }
		public string IP { get; set; }
		public RequestPersistenceModelType Type { get; set; }
		public string Options { get; set; }
		public RequestPersistenceModelStatus Status { get; set; }
		public Guid UUID { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int UserID { get; set; }
		public UserPersistenceModel User { get; set; }
		public ICollection<FilePersistenceModel> Files { get; set; }
		public ICollection<LinkPersistenceModel> Links { get; set; }
	}

	public enum RequestPersistenceModelType : byte {
		Simple = 1,
		Batch  = 2
	}

	public enum RequestPersistenceModelStatus : byte {
		Incomplete   = 0,
		Ready        = 1,
		Acknowledged = 2,
		InProgress   = 3,
		Done         = 4,
		Aborted      = 5,
	}
}