namespace Application.Modules.Tasks;

public record TaskDto(
    Guid Id,
    string Name,
    string? Description,
    Guid ProjectId,
    string ProjectName,
    Guid StateId,
    string State,
    Guid CreatedBy,
    DateTime CreatedAt,
    string CreatedByName,
    Guid? UpdatedBy,
    DateTime? UpdatedAt,
    string? UpdatedByName);
