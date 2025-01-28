using Domain.Abstract;

namespace Application.Users;
public static class UserErrors
{
    public static Error UserNotFound => new Error("User.NotFound", "User not found.");
    public static Error PasswordsDoNotMatch => new Error("User.PasswordsDoNotMatch", "Passwords do not match.");
}
