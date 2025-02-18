namespace Api.Controllers.Identity.Requests;

public sealed record SetPasswordRequest(
    string ResetToken,
    string Password,
    string ConfirmedPassword);