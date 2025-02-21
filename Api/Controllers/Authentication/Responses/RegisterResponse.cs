namespace Api.Controllers.Authentication.Responses;

public record RegisterResponse(Guid Id, TokenResponse Token);
