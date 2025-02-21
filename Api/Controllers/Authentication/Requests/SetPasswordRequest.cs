namespace Api.Controllers.Authentication.Requests;

public sealed record SetPasswordRequest(
    string ResetToken,
    string Password,
    string ConfirmedPassword);