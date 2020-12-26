using System;
using System.Collections.Generic;

namespace Monito.Domain.Entity
{
    public class RequestDomainEntity
    {
        public virtual int ID { get; private set; }
        public virtual string IP { get; private set; }
        public virtual RequestType Type { get; private set; }
        public virtual RequestOptionsDomainEntity Options { get; private set; }
        public virtual RequestStatus Status { get; private set; }
        public virtual Guid UUID { get; private set; }
        public virtual DateTime CreatedAt { get; private set; }
        public virtual DateTime UpdatedAt { get; private set; }
        public virtual int UserID { get; private set; }
        public virtual UserDomainEntity User { get; private set; }
        public virtual ICollection<FileDomainEntity> Files { get; private set; }
        public virtual ICollection<LinkDomainEntity> Links { get; private set; }

        public static RequestDomainEntity Build(
            string requestingIP,
            ICollection<LinkDomainEntity> links,
            RequestType type,
            RequestOptionsDomainEntity options,
            int userID
        ) {
            return new RequestDomainEntity() {
                IP = requestingIP,
                Links = links,
                Type = type,
                Options = options,
                UserID = userID,
                Status = RequestStatus.Ready
            };
        }

        public void Abort() {
            if (Status != RequestStatus.Aborted && Status != RequestStatus.Done) {
                Status = RequestStatus.Aborted;
            }
        }
    }

    public enum RequestType
    {
        Simple = 1,
        Batch = 2
    }

    public enum RequestStatus
    {
        Incomplete = 0,
        Ready = 1,
        Acknowledged = 2,
        InProgress = 3,
        Done = 4,
        Aborted = 5,
    }
}
