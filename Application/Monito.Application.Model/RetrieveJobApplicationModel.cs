using System;
using System.Collections.Generic;

namespace Monito.Application.Model
{
    public class RetrieveJobApplicationModel {
        public int ID { get; set; }
        public RequestType Type { get; set; }

        public RetrieveJobOptionsApplicationModel Options { get; set; }
        public RequestStatus Status { get; set; }
        public int? LinksCount { get; set; }

        public ICollection<RetrieveLinkApplicationModel> Links { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
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

    public class RetrieveJobOptionsApplicationModel {
        public JobHttpMethod Method { get; set; }
        public int Redirects { get; set; }
        public int Threads { get; set; }
        public int Timeout { get; set; }
        public string UserAgent { get; set; }
    }

    public enum JobHttpMethod
    {
        GET = 1,
        HEAD = 2,
    }
}
