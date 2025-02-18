namespace Api.Controllers.Identity.Requests;

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword,
    string ConfirmedPassword);
