namespace Application.Modules.Users.Identity;

public record RegisterUserResponse(
    Guid Id, string UserName, string Email, string Token)
    : UserResponse(Id, UserName, Email);
