using System.Data.Common;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Monito.Database.EF6 {
    public class DbConnectionFactory {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection Build() {
            return new MySqlConnection(_configuration.GetSection("ConnectionString").Value);
        }
    }
}