namespace Api.Users.Dtos;

public record RegisterUserRequest(
    string UserName,
    string Email,
    string Password,
    string CheckingPassword)
{
    public bool IsPasswordsMatch => Password == CheckingPassword;
}
