using Database;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

IConfiguration config = builder.Build();
var connectionString = Environment.GetEnvironmentVariable("ConnectionString")
                       ?? config.GetConnectionString("localdb");

using ILoggerFactory loggerFactory = LoggerFactory.Create(b => b.AddConsole());
var logger = loggerFactory.CreateLogger<Program>();

DatabaseInitializer databaseInitializer = new(
    connectionString!,
    loggerFactory.CreateLogger<DatabaseInitializer>());

databaseInitializer.Initialize();

PrintTables();

void PrintTables()
{
    StringBuilder sb = new();

    sb.AppendLine("Database schema: ");

    using SqlConnection connection = new(connectionString);
    connection.Open();

    using SqlCommand command = connection.CreateCommand();
    command.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME";

    using SqlDataReader reader = command.ExecuteReader();
    while (reader.Read())
    {
        sb.AppendLine(reader.GetString(0));
    }

    logger.LogInformation(sb.ToString());
}