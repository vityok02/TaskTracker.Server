namespace Application.Modules.Users.Identity.RegisterUser;

public record RegisterRequest(
    string UserName,
    string Email,
    string Password,
    string CheckingPassword)
{
    public bool IsPasswordsMatch => Password == CheckingPassword;
}
