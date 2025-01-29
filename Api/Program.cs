using Api.Extensions;
using Api.OptionsSetup;
using Application.Extensions;
using Database;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Persistence.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services
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

app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseAuthorization();

using var scope = app.Services.CreateScope();
var sp = scope.ServiceProvider;

var dbInitializer = new DatabaseInitializer(
    builder.Configuration.GetConnectionString("localdb")!,
    sp.GetRequiredService<ILogger<DatabaseInitializer>>());

dbInitializer.Initialize();

app.MapControllers();

app.Run();
