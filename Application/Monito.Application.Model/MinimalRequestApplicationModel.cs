
using System;
using System.Collections.Generic;

namespace Monito.Application.Model
{
    public class MinimalRequestApplicationModel {
        public virtual int ID { get; set; }
        public virtual RequestApplicationModelType Type { get; set; }
        public virtual RequestOptionsApplicationModel Options { get; set; }
        public virtual RequestApplicationModelStatus Status { get; set; }
        public virtual Guid UUID { get; set; }
        public virtual int UserID { get; set; }
        public virtual DateTime CreatedAt { get; private set; }
        public virtual DateTime UpdatedAt { get; private set; }
        public virtual ICollection<MinimalLinkApplicationModel> Links { get; set; }
    }

    public enum RequestApplicationModelType
    {
        Simple = 1,
        Batch = 2
    }

    public enum RequestApplicationModelStatus
    {
        Incomplete = 0,
        Ready = 1,
        Acknowledged = 2,
        InProgress = 3,
        Done = 4,
        Aborted = 5,
    }

    public class RequestOptionsApplicationModel
    {
        public virtual JobHttpMethod Method { get; set; }
        public virtual int Redirects { get; set; }
        public virtual int Threads { get; set; }
        public virtual int Timeout { get; set; }
        public virtual string UserAgent { get; set; }
    }
}