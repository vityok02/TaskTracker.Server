using Application.Abstract.Messaging;
using Application.Modules.Projects;

namespace Application.Modules.Projects.GetProjectById;

public sealed record GetProjectQuery(
    Guid UserId,
    Guid ProjectId
    )
    : IQuery<ProjectResponse>;
