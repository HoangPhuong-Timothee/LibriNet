using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Libri.DAL.DatabaseContext
{
    public class CustomContext
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public CustomContext(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection")!;
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}