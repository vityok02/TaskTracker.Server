namespace Api.Controllers.Task.Requests;

public record UpdateTaskRequest(
    Guid Id,
    string Name,
    string? Description,
    Guid StateId);