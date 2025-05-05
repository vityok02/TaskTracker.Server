namespace Api.Controllers.User.Responses;

public record UserResponse(
    Guid Id,
    string UserName,
    string Email,
    string? AvatarUrl);
