using System;
using System.Collections.Generic;
using Monito.Persistence.Model.Interface;

namespace Monito.Persistence.Model {
	public class WorkerPersistenceModel : IPrimaryKeyEntity, ITimestampTrackedEntity, IUUIDTrackedEntity {
		public int ID { get; set; }
		public string Hostname { get; set; }
		public Guid UUID { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public ICollection<QueuePersistenceModel> Queues { get; set; }
	}
}