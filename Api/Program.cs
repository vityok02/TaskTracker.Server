using Api.Extensions;
using Application.Extensions;
using Database;
using Infrastructure.Extensions;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;
using Persistence;
using Persistence.Abstractions;
using Persistence.Extensions;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthConfiguration(builder.Configuration)
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddApi()
    .AddSignalR();

builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    string? connection = Environment
        .GetEnvironmentVariable("REDIS") 
            ?? builder.Configuration
                .GetConnectionString("Redis");

    redisOptions.ConfigurationOptions = ConfigurationOptions
        .Parse(connection ?? "localhost:6379");

    redisOptions.ConfigurationOptions.ConnectTimeout = 1000;
    redisOptions.ConfigurationOptions.SyncTimeout = 1000;
    redisOptions.ConfigurationOptions.AbortOnConnectFail = false;
});

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
        await DataSeeder.SeedDataAsync(sqlConnectionFactory);
        await TestingDataSeeder.SeedDataAsync(sqlConnectionFactory);
    }
    catch (Exception)
    {
        logger.LogError("Testing data hasn't been initialized");
    }
}

app.UseResponseCompression();

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    HttpsCompression = HttpsCompressionMode.Compress,
    OnPrepareResponse = context =>
        context.Context.Response.Headers[HeaderNames.CacheControl] =
            $"public,max-age={86_400}"
});

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

app.MapHub<CommentsHub>(HubEndpoints.CommentsHub);
app.MapHub<NotificationHub>(HubEndpoints.NotificationHub);

await app.RunAsync();
