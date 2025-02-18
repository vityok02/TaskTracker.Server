using Domain.Shared;

namespace Domain.Errors;

public static class UserErrors
{
    public static Error NotFound
        => new("User.NotFound", "User not found.");

    public static Error InvalidCredentials
        => new("InvalidCredentials", "Invalid email or password.");

    public static Error AlreadyExists
        => new("User.AlreadyExists", "User with such email already exists.");

    public static Error Unauthorized
        => new("Unauthorized", "User is unauthorized.");

    public static Error InvalidToken
        => new("InvalidToken", "Token is invalid.");
}
