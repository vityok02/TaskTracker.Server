using Application.Abstract.Messaging;
using Application.Modules.Users.Identity.RegisterUser;

namespace Application.Modules.Identity.Register;

public sealed record RegisterCommand(
    string Username,
    string Email,
    string Password,
    string ConfirmedPassword)
    : ICommand<RegisterResponse>;
