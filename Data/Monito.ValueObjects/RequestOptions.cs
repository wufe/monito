using System;
using System.Text.Json.Serialization;
using Monito.Enums;

namespace Monito.ValueObjects
{
    public class RequestOptions
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public JobHttpMethod Method { get; set; }
        public int Redirects { get; set; }
        public int Threads { get; set; }
        public int Timeout { get; set; }
        public string UserAgent { get; set; }
    }
}
