using Application.Abstract.Messaging;
using Domain.Abstract;

namespace Application.Modules.Projects.GetPagedProjects;

public sealed record GetPagedProjectsQuery(
    int? CurrentPageNumber,
    int? PageSize,
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    Guid UserId)
    : IQuery<PagedList<ProjectDto>>;
