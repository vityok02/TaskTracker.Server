using Application.Abstract.Messaging;

namespace Application.Modules.Identity.Login;

public sealed record LoginCommand(string Email, string Password)
    : ICommand<TokenDto>;
