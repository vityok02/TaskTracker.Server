using Application.Abstract.Messaging;

namespace Application.Modules.Projects.GetProjectById;

public sealed record GetProjectQuery(
    Guid UserId,
    Guid ProjectId
    )
    : IQuery<ProjectDto>;
