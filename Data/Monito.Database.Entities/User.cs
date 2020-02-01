using System;
using System.Collections.Generic;
using Monito.Database.Entities.Interface;

namespace Monito.Database.Entities {
    public class User : IPrimaryKeyEntity, ITimestampTrackedEntity, IUUIDTrackedEntity {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string IP { get; set; }
        public Guid UUID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Request> Requests { get; set; }
    }
}