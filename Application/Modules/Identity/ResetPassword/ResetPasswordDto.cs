namespace Application.Modules.Identity.ResetPassword;

public record ResetPasswordDto(string Email, string ResetToken);
