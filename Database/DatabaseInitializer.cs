using DbUp;
using DbUp.Engine;
using Microsoft.Extensions.Logging;

namespace Database;

public class DatabaseInitializer
{
    private readonly string _connectionString;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(
        string connectionString,
        ILogger<DatabaseInitializer> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public void Initialize()
    {
        EnsureDatabase.For.SqlDatabase(_connectionString);

        var upgrader = DeployChanges.To
            .SqlDatabase(_connectionString)
            .WithTransaction()
            .WithScriptsEmbeddedInAssembly(typeof(DatabaseInitializer).Assembly)
            .LogTo(_logger)
            .Build();

        if (!upgrader.IsUpgradeRequired())
        {
            _logger.LogInformation("Database is already updated");
            return;
        }
        
        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            _logger.LogError("A script error occurred at {ScriptName}", result.ErrorScript.Name);
            _logger.LogError("Error script: {Script}", result.ErrorScript.Contents);
            return;
        }

        _logger.LogInformation("Database successfully migrated");
    }
}
