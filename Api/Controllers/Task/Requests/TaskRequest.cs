namespace Api.Controllers.Task.Requests;

public record TaskRequest(
    string Name,
    string? Description,
    Guid StateId);
