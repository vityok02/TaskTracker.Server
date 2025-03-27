using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Abstract;
using Domain.Models;
using Domain.Shared;

namespace Application.Modules.Projects.GetPagedProjects;

public sealed record GetPagedProjectsQuery(
    int? CurrentPageNumber,
    int? PageSize,
    Guid UserId)
    : IQuery<PagedList<ProjectDto>>;

internal sealed class GetPagedProjectsQueryHandler
    : IQueryHandler<GetPagedProjectsQuery, PagedList<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetPagedProjectsQueryHandler(
        IProjectRepository projectRepository,
        IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<ProjectDto>>> Handle(
        GetPagedProjectsQuery query,
        CancellationToken cancellationToken)
    {
        var currentPageNumber = query.CurrentPageNumber ?? 1;

        var pageSize = query.PageSize ?? 10;
        
        int maxPageSize = 50;

        pageSize = pageSize < maxPageSize
            ? pageSize
            : maxPageSize;

        var projects = await _projectRepository
            .GetPagedAsync(
                currentPageNumber,
                pageSize,
                query.UserId);

        return Result<PagedList<ProjectDto>>.Success(_mapper
            .Map<PagedList<ProjectDto>>(projects));
    }
}
