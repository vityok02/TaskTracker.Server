using Microsoft.Data.SqlClient;
using Persistence.Abstractions;

namespace Persistence;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConnectionStringProvider _connectionStringProvider;

    public SqlConnectionFactory(IConnectionStringProvider connectionStringProvider)
    {
        _connectionStringProvider = connectionStringProvider;
    }

    public SqlConnection Create()
    {
        return new SqlConnection(_connectionStringProvider.GetConnectionString());
    }
}