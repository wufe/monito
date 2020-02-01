using System;
using System.Linq;
using System.Linq.Expressions;
using Monito.Database.Entities.Interface;

namespace Monito.Repository.Interface
{
    public interface IReadRepository<T>
        where T: IPrimaryKeyEntity
    {
        T Find(int ID);
        IQueryable<T> FindAll(Expression<Func<T, bool>> selector);
        IQueryable<T> FindAll();
        IQueryable<T> AsNoTracking();
    }
}
