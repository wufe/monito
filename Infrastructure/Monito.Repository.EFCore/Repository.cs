using Microsoft.EntityFrameworkCore;
using Monito.Database.Entities.Interface;
using Monito.Repository.EFCore;
using Monito.Repository.Interface;

namespace Monito.Repository.EFCore {
	public class Repository<T> : ReadRepository<T>, IRepository<T>
		where T : class, IPrimaryKeyEntity
	{
		public Repository(DbContext context) : base(context)
		{
		}

		public void Insert(T entity)
		{
			_context.Add(entity);
		}

		public void Update(T entity)
		{
			_context.Update(entity);
		}

		public void SaveChanges() {
			_context.SaveChanges();
		}
	}
}