using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
namespace ApiServerDemo.Models
{
    public class ValleyHospitalContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public ValleyHospitalContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
