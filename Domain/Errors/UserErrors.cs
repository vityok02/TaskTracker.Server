using Domain.Shared;

namespace Domain.Errors;

public static class UserErrors
{
    public static Error NotFound
        => new("User.NotFound", "User not found.");

    public static Error InvalidCredentials
        => new("InvalidCredentials", "Invalid email or password.");

    public static Error Unauthorized
        => new("Unauthorized", "User is unauthorized.");

    public static Error Forbidden
        => new("Forbidden", "Aunthenticated user is not authorized.");

    public static Error EmailAlreadyExists
        => new("User.AlreadyExists", "User with such email already exists.");

    public static Error NameAlreadyExists
        => new("User.AlreadyExists", "User with such name already exists.");

    public static Error InvalidToken
        => new("InvalidToken", "Token is invalid.");
}
