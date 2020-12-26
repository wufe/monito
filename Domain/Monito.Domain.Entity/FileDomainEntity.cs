using System;

namespace Monito.Domain.Entity
{
    public class FileDomainEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public FileType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int RequestID { get; set; }
        public RequestDomainEntity Request { get; set; }
    }

    public enum FileType
    {
        TXT = 1,
        CSV = 2
    }
}