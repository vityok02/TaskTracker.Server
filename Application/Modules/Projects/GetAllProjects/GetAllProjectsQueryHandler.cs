using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Shared;

namespace Application.Modules.Projects.GetAllProjects;

internal sealed class GetAllProjectsQueryHandler
    : IQueryHandler<GetAllProjectsQuery, IEnumerable<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetAllProjectsQueryHandler(
        IProjectRepository projectRepository,
        IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ProjectDto>>> Handle(
        GetAllProjectsQuery query,
        CancellationToken cancellationToken)
    {
        var projects = await _projectRepository
            .GetAllByUserIdAsync(query.UserId);

        return Result<IEnumerable<ProjectDto>>
            .Success(_mapper.Map<IEnumerable<ProjectDto>>(projects));
    }
}