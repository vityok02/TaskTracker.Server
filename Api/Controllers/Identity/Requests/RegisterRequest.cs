namespace Api.Controllers.Identity.Requests;

public record RegisterRequest(
    string UserName,
    string Email,
    string Password,
    string ConfirmedPassword)
{
}
