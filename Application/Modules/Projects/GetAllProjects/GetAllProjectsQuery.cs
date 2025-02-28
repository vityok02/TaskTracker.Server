using Application.Abstract.Messaging;

namespace Application.Modules.Projects.GetAllProjects;

public sealed record GetAllProjectsQuery(Guid UserId)
    : IQuery<IEnumerable<ProjectDto>>;
