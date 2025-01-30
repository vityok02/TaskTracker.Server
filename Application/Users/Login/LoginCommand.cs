using Application.Abstract.Messaging;

namespace Application.Users.Login;

public sealed record LoginCommand(LoginRequest LoginRequest) : ICommand<string>;
