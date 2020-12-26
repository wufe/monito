using System;
using Monito.Persistence.Model.Interface;

namespace Monito.Persistence.Model {
	public class QueuePersistenceModel : IPrimaryKeyEntity, ITimestampTrackedEntity {
		public int ID { get; set; }
		public QueueType Type { get; set; }
		public QueueStatus Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int WorkerID { get; set; }
		public WorkerPersistenceModel Worker { get; set; }
	}

	public enum QueueType : byte {
		Basic    = 1,
		Priority = 2,
		Simple   = 3
	}

	public enum QueueStatus : byte {
		Idle    = 0,
		Busy    = 1,
		Offline = 2
	}
}