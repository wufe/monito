using Microsoft.Extensions.DependencyInjection;
using Monito.Persistence.Database.MySQL;
using Monito.Persistence.Repository.Interface;
using Monito.Persistence.Repository.MySQL;

namespace Monito.Persistence.Configuration.DependencyInjection
{
    public static class Extensions {
        public static void AddMonitoDbContext(this IServiceCollection services) {
            services.AddTransient<MonitoDbContext>();
        }

        public static void AddGenericRepositories(this IServiceCollection services) {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IReadRepository<>), typeof(ReadRepository<>));
        }
    }
}