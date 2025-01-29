using Domain.Abstract;

namespace Application.Users;

public static class UserErrors
{
    public static Error UserNotFound => new ("User.NotFound", "User not found.");
    public static Error PasswordsDoNotMatch => new ("User.PasswordsDoNotMatch", "Passwords do not match.");
    public static Error ProjectAlreadyExists => new ("User.ProjectAlreadyExists", "Project already exists.");
}
