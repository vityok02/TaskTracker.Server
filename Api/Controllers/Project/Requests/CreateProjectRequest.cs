namespace Api.Controllers.Project.Requests;

public record CreateProjectRequest(
    string Name,
    string? Description,
    DateTime? StartDate);
