namespace Api.Controllers.State.Requests;

public record UpdateStateRequest(
    string Name,
    string? Description,
    int? Number);
