using System;

namespace Monito.Application.Model
{
    public class LinkApplicationModel : MinimalLinkApplicationModel {
        public virtual LinkApplicationModelStatus Status { get; set; }
        public virtual string AdditionalData { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public int RequestID { get; set; }
    }
}