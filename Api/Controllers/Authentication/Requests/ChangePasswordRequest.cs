namespace Api.Controllers.Authentication.Requests;

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword,
    string ConfirmedPassword);
