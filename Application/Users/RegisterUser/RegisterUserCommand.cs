using Api.Users.Dtos;
using Application.Abstract.Messaging;
using Application.Users.GetUser;

namespace Application.Users.RegisterUser;

public sealed record RegisterUserCommand(RegisterUserRequest UserDto)
    : ICommand<UserResponse>;
