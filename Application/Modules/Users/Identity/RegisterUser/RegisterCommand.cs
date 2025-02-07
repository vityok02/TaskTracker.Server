using Application.Abstract.Messaging;

namespace Application.Modules.Users.Identity.RegisterUser;

public sealed record RegisterCommand(RegisterRequest UserDto)
    : ICommand<RegisterResponse>;
