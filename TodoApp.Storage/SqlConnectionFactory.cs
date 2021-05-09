using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Storage
{
   public class SqlConnectionFactory
    {
        private readonly IConfiguration configuration;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public SqlConnection CreateConnection()
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            return new SqlConnection(connectionString);
        }

    }
}
