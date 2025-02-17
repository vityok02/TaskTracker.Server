namespace Api.Controllers.Identity.Responses;

public record TokenResponse(string Token, string ExpiresIn);
