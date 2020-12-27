using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Monito.Domain.Entity;

namespace Monito.Web.Models
{
    public class SaveJobInputModel
    {

        public string Links { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public JobHttpMethod Method { get; set; }

        [Required]
        [Range(0, 10)]
        public int Redirects { get; set; }

        [Required]
        [Range(1, 4)]
        public int Threads { get; set; }

        [Required]
        [Range(1000, 20000)]
        public int Timeout { get; set; }

        [Required]
        public string UserAgent { get; set; }
    }
}