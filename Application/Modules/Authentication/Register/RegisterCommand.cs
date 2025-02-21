using Application.Abstract.Messaging;

namespace Application.Modules.Authentication.Register;

public sealed record RegisterCommand(
    string Username,
    string Email,
    string Password,
    string ConfirmedPassword)
    : ICommand<RegisterDto>;
