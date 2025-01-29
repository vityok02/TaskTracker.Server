using Application.Abstract.Messaging;

namespace Application.Users.Login;

public sealed record LoginCommand(string Email) : ICommand<string>;
