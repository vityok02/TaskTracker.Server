namespace Api.Controllers.Task.Requests;

public record TaskRequest(
    string Name,
    string? Description,
    DateTime? StartDate,
    DateTime? EndDate,
    Guid StateId);
