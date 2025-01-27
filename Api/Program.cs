using Api.Extensions;
using Application.Extensions;
using Database;
using Infrastructure.Extensions;
using Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence(builder.Configuration)
    .AddApi();

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

using var scope = app.Services.CreateScope();
var sp = scope.ServiceProvider;
var dbInitializer = new DatabaseInitializer(
    builder.Configuration.GetConnectionString("localdb")!,
    sp.GetRequiredService<ILogger<DatabaseInitializer>>());

dbInitializer.Initialize();

app.MapControllers();

app.Run();
