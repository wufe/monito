using Monito.Database.Entities.Interface;

namespace Monito.Repository.Interface {
    public interface IRepository<T> : IReadRepository<T>
        where T: IPrimaryKeyEntity
    {
        void Insert(T entity);
        void Update(T entity);
        void SaveChanges();
    }
}