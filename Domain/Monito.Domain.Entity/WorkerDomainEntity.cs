using System;
using System.Collections.Generic;

namespace Monito.Domain.Entity
{
    public class WorkerDomainEntity
    {
        public int ID { get; set; }
        public string Hostname { get; set; }
        public Guid UUID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<QueueDomainEntity> Queues { get; set; }
    }
}