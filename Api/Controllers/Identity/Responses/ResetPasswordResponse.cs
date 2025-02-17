namespace Api.Controllers.Identity.Responses;

public record ResetPasswordResponse(string Email, string ResetToken);
