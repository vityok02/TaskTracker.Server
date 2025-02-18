using Application.Abstract.Messaging;

namespace Application.Modules.Identity.ResetPassword;

public sealed record ResetPasswordCommand(string Email) : ICommand<ResetPasswordDto>;
