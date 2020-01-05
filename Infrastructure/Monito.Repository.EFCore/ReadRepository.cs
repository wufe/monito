using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Monito.Database.Entities.Interface;
using Monito.Repository.Interface;

namespace Monito.Repository.EFCore
{
	public class ReadRepository<T> : IReadRepository<T>
		where T : class, IPrimaryKeyEntity
	{
		protected readonly DbContext _context;

		public ReadRepository(DbContext context)
        {
            _context = context;
        }

		public IQueryable<T> AsNoTracking() =>
			_context
				.Set<T>()
				.AsNoTracking();

		public T Find(int ID) =>
            _context
				.Set<T>()
				.FirstOrDefault(x => x.ID == ID);

		public IQueryable<T> FindAll(Func<T, bool> selector) =>
            _context
				.Set<T>()
				.Where(selector)
				.AsQueryable();

		public IQueryable<T> FindAll() =>
            _context
                .Set<T>();
	}
}
