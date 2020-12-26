using System;
using System.Collections.Generic;

namespace Monito.Domain.Entity
{
    public class UserDomainEntity
    {
        public virtual int ID { get; private set; }
        public virtual string Name { get; private set; }
        public virtual string Email { get; private set; }
        public virtual string Password { get; private set; }
        public virtual string IP { get; private set; }
        public virtual Guid UUID { get; private set; }
        public virtual DateTime CreatedAt { get; private set; }
        public virtual DateTime UpdatedAt { get; private set; }
        public virtual ICollection<RequestDomainEntity> Requests { get; private set; }

        public static UserDomainEntity Build(string ip) {
            return new UserDomainEntity() {
                IP = ip
            };
        }
    }
}