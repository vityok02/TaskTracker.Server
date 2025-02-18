using Application.Abstract.Messaging;

namespace Application.Modules.Identity.ChangePassword;

public sealed record ChangePasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword,
    string ConfirmedPassword)
    : ICommand;
