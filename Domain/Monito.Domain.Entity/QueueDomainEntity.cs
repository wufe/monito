using System;

namespace Monito.Domain.Entity
{
    public class QueueDomainEntity
    {
        public int ID { get; set; }
        public QueueType Type { get; set; }
        public QueueStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int WorkerID { get; set; }
        public WorkerDomainEntity Worker { get; set; }
    }

    public enum QueueType
    {
        Basic = 1,
        Priority = 2,
        Simple = 3
    }

    public enum QueueStatus
    {
        Idle = 0,
        Busy = 1,
        Offline = 2
    }
}