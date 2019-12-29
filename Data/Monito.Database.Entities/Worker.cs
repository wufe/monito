using System;
using System.Collections.Generic;
using Monito.Database.Entities.Interface;

namespace Monito.Database.Entities {
	public class Worker : IPrimaryKeyEntity, ITimestampTrackedEntity, IUUIDTrackedEntity {
		public int ID { get; set; }
		public string Hostname { get; set; }
		public Guid UUID { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public virtual ICollection<Queue> Queues { get; set; }
	}
}