using System;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Monito.Database.Interface;
using MySql.Data.MySqlClient;

namespace Monito.Database.EF6
{
    public class Context : DbContext, IDbContext
    {
        private readonly IConfiguration _configuration;

        public Context(IConfiguration configuration, DbConnection connection)
            : base(connection, false)
        {
            
            _configuration = configuration;
        }
    }
}
