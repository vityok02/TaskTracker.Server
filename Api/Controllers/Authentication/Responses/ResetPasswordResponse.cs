namespace Api.Controllers.Authentication.Responses;

public record ResetPasswordResponse(string Email, string ResetToken);
