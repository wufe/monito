using System;
using System.Linq;
using Monito.Database.Entities.Interface;

namespace Monito.Repository.Interface
{
    public interface IReadRepository<T>
        where T: IIdentityEntity
    {
        T Find(int ID);
        IQueryable<T> FindAll(Func<T, bool> selector);
    }
}
