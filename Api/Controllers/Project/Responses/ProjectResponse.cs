namespace Api.Controllers.Project.Responses;

public record ProjectResponse(
    Guid Id,
    string Name,
    string? Description,
    string CreatedBy,
    DateTime CreatedAt,
    string? UpdatedBy,
    DateTime? UpdatedAt);
