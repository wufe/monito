using System;

namespace Monito.Domain.Entity
{
    public class LinkDomainEntity
    {
        public virtual int ID { get; private set; }
        public virtual string URL { get; private set; }
        public virtual LinkStatus Status { get; private set; }
        public virtual string Output { get; private set; }
        public virtual int StatusCode { get; private set; }
        public virtual string AdditionalData { get; private set; }
        public virtual Guid UUID { get; private set; }
        public virtual DateTime CreatedAt { get; private set; }
        public virtual DateTime UpdatedAt { get; private set; }
        public virtual int? RedirectsFromLinkId { get; private set; }
        public virtual LinkDomainEntity RedirectsFrom { get; private set; }
        public virtual LinkDomainEntity RedirectsTo { get; private set; }
        public virtual int RequestID { get; private set; }
        public virtual RequestDomainEntity Request { get; private set; }

        public static LinkDomainEntity Build(string url) {
            return new LinkDomainEntity() {
                URL = url,
                Status = LinkStatus.Idle
            };
        }
    }

    public enum LinkStatus
    {
        Idle = 0,
        Acknowledged = 1,
        InProgress = 2,
        Done = 3
    }
}