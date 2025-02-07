namespace Application.Modules.Users.Identity.RegisterUser;

public record RegisterRequest(
    string UserName,
    string Email,
    string Password,
    string ConfirmedPassword)
{
    public bool IsPasswordsMatch()
    {
        return Password == ConfirmedPassword;
    }
}
