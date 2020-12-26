using System;
using System.Linq;
using System.Linq.Expressions;
using Monito.Persistence.Model.Interface;

namespace Monito.Persistence.Repository.Interface
{
    public interface IReadRepository<T>
        where T: IPrimaryKeyEntity
    {
        T Find(int ID);
        IQueryable<T> FindAll(Expression<Func<T, bool>> selector);
        IQueryable<T> FindAll();
        IQueryable<T> AsNoTracking();
        IQueryable<T> Include<TProperty>(Expression<Func<T, TProperty>> selector);
    }
}
