using Application.Abstract.Messaging;
using Domain.Abstract;

namespace Application.Modules.Projects.GetPagedProjects;

public sealed record GetPagedProjectsQuery(
    int? CurrentPageNumber,
    int? PageSize,  
    Guid UserId)
    : IQuery<PagedList<ProjectDto>>;
