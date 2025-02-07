namespace Application.Modules.Users.Identity.RegisterUser;

public record RegisterResponse(
    Guid Id, TokenResponse Token);
