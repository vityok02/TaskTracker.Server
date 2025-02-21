using Application.Abstract.Messaging;

namespace Application.Modules.Authentication.ResetPassword;

public sealed record ResetPasswordCommand(string Email) : ICommand<ResetPasswordDto>;
