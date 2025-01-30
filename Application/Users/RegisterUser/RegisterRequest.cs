namespace Api.Users.Dtos;

public record RegisterRequest(
    string UserName,
    string Email,
    string Password,
    string CheckingPassword)
{
    public bool IsPasswordsMatch => Password == CheckingPassword;
}
