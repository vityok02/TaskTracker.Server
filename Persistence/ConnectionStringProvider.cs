using Microsoft.Extensions.Configuration;
using Persistence.Abstractions;

namespace Persistence;

public class ConnectionStringProvider : IConnectionStringProvider
{
    private readonly IConfiguration _configuration;

    public ConnectionStringProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetConnectionString()
    {
        return Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")
            ?? _configuration.GetConnectionString("localdb")!;
    }
}
