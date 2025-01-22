using Application.Extensions;
using Infrastructure.Extensions;
using Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence();

app.MapGet("/", () => "Welcome to task tracker!");

app.Run();
