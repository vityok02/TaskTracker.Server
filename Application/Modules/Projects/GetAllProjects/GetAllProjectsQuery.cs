using Application.Abstract.Messaging;

namespace Application.Modules.Projects.GetAllProjects;

public sealed record GetAllProjectsQuery(Guid MemberId)
    : IQuery<IEnumerable<ProjectDto>>;
