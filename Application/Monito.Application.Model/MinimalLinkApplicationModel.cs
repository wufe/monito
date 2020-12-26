using System;

namespace Monito.Application.Model
{
    public class MinimalLinkApplicationModel {
        public virtual int ID { get; set; }
        public virtual Guid UUID { get; set; }
        public virtual string URL { get; set; }
        public virtual string Output { get; set; }
        public virtual int StatusCode { get; set; }
        public virtual int? RedirectsFromLinkId { get; set; }
        public virtual int? RedirectsToLinkId { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
    }

    public enum LinkApplicationModelStatus
    {
        Idle = 0,
        Acknowledged = 1,
        InProgress = 2,
        Done = 3
    }
}