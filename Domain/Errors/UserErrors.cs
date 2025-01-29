using Domain.Abstract;

namespace Domain.Errors;

public static class UserErrors
{
    public static Error UserNotFound
        => new("User.NotFound", "User not found.");

    public static Error PasswordsDoNotMatch
        => new("User.PasswordsDoNotMatch", "Passwords do not match.");

    public static Func<Guid, string, Error> ProjectAlreadyExists => (userId, name)
        => new("User.ProjectAlreadyExists", $"User with id: '{userId}' already has project with name '{name}'");

    public static Error InvalidCredentials
        => new("User.InvalidCredentials", "Invalid credentials.");
}
