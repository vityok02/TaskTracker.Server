using Application.Abstract.Messaging;

namespace Application.Modules.Users.Identity.Login;

public sealed record LoginCommand(LoginRequest LoginRequest)
    : ICommand<string>;
