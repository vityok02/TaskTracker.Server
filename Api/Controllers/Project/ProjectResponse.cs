namespace Api.Controllers.Project;

public record ProjectResponse(
    Guid Id,
    string Name,
    string? Description,
    string CreatedBy,
    DateTime CreatedAt,
    string? UpdatedBy,
    DateTime? UpdatedAt);
