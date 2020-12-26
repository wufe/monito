using System;
using System.Collections.Generic;
using Monito.Persistence.Model.Interface;

namespace Monito.Persistence.Model {
    public class UserPersistenceModel : IPrimaryKeyEntity, ITimestampTrackedEntity, IUUIDTrackedEntity {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string IP { get; set; }
        public Guid UUID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<RequestPersistenceModel> Requests { get; set; }
    }
}