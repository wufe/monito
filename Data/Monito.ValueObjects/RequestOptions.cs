using System;
using Monito.Enums;

namespace Monito.ValueObjects
{
    public class RequestOptions
    {
        public JobHttpMethod Method { get; set; }
        public int Redirects { get; set; }
        public int Threads { get; set; }
        public int Timeout { get; set; }
        public string UserAgent { get; set; }
    }
}
