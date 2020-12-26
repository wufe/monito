using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Monito.Persistence.Database.MySQL;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace Monito.Web.Configuration.DependencyInjection
{
    public static class Extensions {
        public static void AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DbContext, MonitoDbContext>(options =>
                options
                    .UseMySql(connectionString, mysqlOptions =>
                        mysqlOptions.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql))));
        }
    }
}