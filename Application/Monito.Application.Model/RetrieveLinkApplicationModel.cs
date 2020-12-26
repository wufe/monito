using System;

namespace Monito.Application.Model
{
    public class RetrieveLinkApplicationModel : RetrieveBriefLinkApplicationModel
    {
        public Guid UUID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int RequestID { get; set; }
    }
}