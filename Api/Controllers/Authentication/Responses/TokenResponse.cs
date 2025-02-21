namespace Api.Controllers.Authentication.Responses;

public record TokenResponse(string Token, string ExpiresIn);
