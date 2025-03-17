namespace Api.Controllers.State.Requests;

public record CreateStateRequest(
    string Name,
    string? Description);
