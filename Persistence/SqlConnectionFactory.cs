using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("localdb")
            ?? throw new ApplicationException("Connection string is missing");
    }

    public IDbConnection Create()
    {
        return new SqlConnection(_connectionString);
    }
}