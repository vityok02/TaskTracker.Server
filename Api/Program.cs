using Api.Extensions;
using Api.Users;
using Application.Extensions;
using Application.Interfaces;
using Database;
using Domain;
using Infrastructure.Extensions;
using Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence(builder.Configuration)
    .AddApi();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var sp = scope.ServiceProvider;
var dbInitializer = new DatabaseInitializer(
    builder.Configuration.GetConnectionString("localdb")!,
    sp.GetRequiredService<ILogger<DatabaseInitializer>>());

dbInitializer.Initialize();

app.MapGet("/", () => "Welcome to task tracker!");

app.MapPost("/users", async (
    IRepository<User, Guid> userRepository,
    CreateUserDto userDto) =>
{
    User user = User.Create(Guid.NewGuid(), userDto.UserName);

    await userRepository.CreateAsync(user);
});

app.Run();
