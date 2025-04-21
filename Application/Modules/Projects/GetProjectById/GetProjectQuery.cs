using Application.Abstract.Messaging;

namespace Application.Modules.Projects.GetProjectById;

public sealed record GetProjectQuery(
    Guid ProjectId,
    Guid UserId)
    : IQuery<ProjectDto>;
