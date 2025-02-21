namespace Api.Controllers.Authentication.Requests;

public record RegisterRequest(
    string UserName,
    string Email,
    string Password,
    string ConfirmedPassword)
{
}
