using Monito.Database.Entities.Interface;

namespace Monito.Repository.Interface {
    public interface IRepository<T> : IReadRepository<T>
        where T: IIdentityEntity
    {
        void Insert(T entity);
        void Update(T entity);
    }
}