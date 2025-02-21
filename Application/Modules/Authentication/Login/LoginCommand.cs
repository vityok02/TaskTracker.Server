using Application.Abstract.Messaging;

namespace Application.Modules.Authentication.Login;

public sealed record LoginCommand(string Email, string Password)
    : ICommand<TokenDto>;
