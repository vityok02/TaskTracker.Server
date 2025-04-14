using Api.Extensions;
using Api.Hubs;
using Application.Extensions;
using Database;
using Infrastructure.Extensions;
using Persistence;
using Persistence.Abstractions;
using Persistence.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthConfiguration(builder.Configuration)
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddApi()
    .AddSignalR();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

using var scope = app.Services.CreateScope();
var sp = scope.ServiceProvider;

var logger = sp
    .GetRequiredService<ILogger<DatabaseInitializer>>();

var connectionStringProvider = sp
    .GetRequiredService<IConnectionStringProvider>();

var dbInitializer = new DatabaseInitializer(
    connectionStringProvider
        .GetConnectionString(),
    logger);

dbInitializer.Initialize();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        o.RoutePrefix = string.Empty;
    });

    var sqlConnectionFactory = sp
        .GetRequiredService<ISqlConnectionFactory>();

    try
    {
        await TestingDataSeeder.SeedData(sqlConnectionFactory);
    }
    catch (Exception)
    {
        logger.LogError("Testing data hasn't been initialized");
    }
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();
app.MapHub<CommentsHub>("/hubs/comments");

await app.RunAsync();
