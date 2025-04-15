using Application.Abstract.Messaging;

namespace Application.Modules.Projects.UpdateProject;

public sealed record UpdateProjectCommand(
    Guid UserId,
    Guid ProjectId,
    string Name,
    string? Description,
    DateTime? StartDate,
    DateTime? EndDate)
    : ICommand;
