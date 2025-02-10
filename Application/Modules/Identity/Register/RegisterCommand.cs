using Application.Abstract.Messaging;
using Application.Modules.Users.Identity.RegisterUser;

namespace Application.Modules.Identity.Register;

public sealed record RegisterCommand(
    string UserName,
    string Email,
    string Password,
    string ConfirmedPassword)
    : ICommand<RegisterResponse>;
