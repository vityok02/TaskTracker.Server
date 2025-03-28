using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Abstract;
using Domain.Shared;

namespace Application.Modules.Projects.GetPagedProjects;

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
        int currentPageNumber = query.CurrentPageNumber is not null or > 0 
            ? query.CurrentPageNumber.Value 
            : 1;

        int pageSize = query.PageSize is not null or > 10
            ? query.PageSize.Value 
            : 10;
        
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
