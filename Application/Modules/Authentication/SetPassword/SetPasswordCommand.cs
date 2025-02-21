using Application.Abstract.Messaging;

namespace Application.Modules.Authentication.SetPassword;

public sealed record SetPasswordCommand(
    string ResetToken,
    string Password,
    string ConfirmedPassword)
    : ICommand<TokenDto>;
