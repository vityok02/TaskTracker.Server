namespace Api.Controllers.Identity.Requests;

public record ChangePasswordRequest(
    Guid UserId,
    string CurrentPassword,
    string NewPassword,
    string ConfirmedPassword);
