namespace Api.Controllers.Project.Requests;

public record UpdateProjectRequest(
    string Name,
    string? Description,
    DateTime? StartDate,
    DateTime? EndDate);
