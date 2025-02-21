namespace Application.Modules.Authentication.ResetPassword;

public record ResetPasswordDto(string Email, string ResetToken);
