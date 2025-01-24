using Api.Users;
using Application.Extensions;
using Application.Interfaces;
using Domain;
using Infrastructure.Extensions;
using Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Welcome to task tracker!");

app.MapPost("/users", async (
    IRepository<User> userRepository,
    CreateUserDto userDto) =>
{
    User user = User.Create(Guid.NewGuid(), userDto.UserName);

    await userRepository.CreateAsync(user);
});

app.Run();
