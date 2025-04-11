namespace Api.Controllers.State.Requests;

public record StateRequest(
    string Name,
    string? Description,
    string? Color);
