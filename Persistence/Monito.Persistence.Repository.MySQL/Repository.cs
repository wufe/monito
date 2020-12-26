using System.Linq;
using Microsoft.EntityFrameworkCore;
using Monito.Persistence.Model.Interface;
using Monito.Persistence.Repository.Interface;

namespace Monito.Persistence.Repository.MySQL {
	public class Repository<T> : ReadRepository<T>, IRepository<T>
		where T : class, IPrimaryKeyEntity
	{
		public Repository(DbContext context) : base(context)
		{
		}

		public void Insert(T entity)
		{
			_context.Add(entity);
			_context.SaveChanges();
		}

		public void Update(T entity)
		{
			_context.Update(entity);
            _context.SaveChanges();
		}
	}
}