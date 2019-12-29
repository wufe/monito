using System;
using Monito.Database.Entities.Interface;

namespace Monito.Database.Entities {
    public class User : IIdentityEntity {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string IP { get; set; }
        public Guid UUID { get; set; }
    }
}