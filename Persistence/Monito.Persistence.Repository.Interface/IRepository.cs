using Monito.Persistence.Model.Interface;

namespace Monito.Persistence.Repository.Interface {
    public interface IRepository<T> : IReadRepository<T>
        where T: IPrimaryKeyEntity
    {
        void Insert(T entity);
        void Update(T entity);
    }
}