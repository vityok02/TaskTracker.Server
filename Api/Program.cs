using Api.Extensions;
using Application.Extensions;
using Database;
using Infrastructure.Extensions;
using Persistence.Abstractions;
using Persistence.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthConfiguration(builder.Configuration)
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence(builder.Configuration)
    .AddApi();


builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        o.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

using var scope = app.Services.CreateScope();
var sp = scope.ServiceProvider;

var connectionStringProvider = sp
    .GetRequiredService<IConnectionStringProvider>();

var dbInitializer = new DatabaseInitializer(
    connectionStringProvider.GetConnectionString(),
    sp.GetRequiredService<ILogger<DatabaseInitializer>>());

dbInitializer.Initialize();

await app.RunAsync();
