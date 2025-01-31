using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Persistence.Abstractions;
using System.Data;

namespace Persistence;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("localdb")!;
    }

    public SqlConnection Create()
    {
        return new SqlConnection(_connectionString);
    }
}