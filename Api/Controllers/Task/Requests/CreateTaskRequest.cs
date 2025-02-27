namespace Api.Controllers.Task.Requests;

public record CreateTaskRequest(
    string Name,
    string? Description,
    Guid StateId);
