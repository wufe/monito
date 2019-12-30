using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Monito.Database.EFCore;
using Monito.Domain.Service;
using Monito.Domain.Service.Interface;
using Monito.Repository.EFCore;
using Monito.Repository.Interface;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace Monito.Web.Integration {
	public static class DependencyInjectionExtensions {

		public static void AddDatabase(this IServiceCollection services, string connectionString) {
			services.AddDbContext<DbContext, MonitoContext>(options =>
				options
					.UseLazyLoadingProxies()
					.UseMySql(connectionString, mysqlOptions =>
						mysqlOptions.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql))));
		}

		public static void AddDomainServices(this IServiceCollection services) {
			services
				.AddScoped<IUserService, UserService>();
			services
				.AddScoped<IRequestService, RequestService>();
		}

		public static void AddRepositories(this IServiceCollection services) {
			services
				.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
			services
				.AddScoped(typeof(IRepository<>), typeof(Repository<>));
		}
	}
}