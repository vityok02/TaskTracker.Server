using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Shared;

namespace Application.Modules.Projects.GetAllProjects;

public sealed record GetAllProjectsQuery(Guid MemberId)
    : IQuery<IEnumerable<ProjectResponse>>;

internal sealed class GetAllProjectsQueryHandler
    : IQueryHandler<GetAllProjectsQuery, IEnumerable<ProjectResponse>>
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

    public async Task<Result<IEnumerable<ProjectResponse>>> Handle(
        GetAllProjectsQuery query,
        CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAllAsync(query.MemberId);

        return projects
            .Select(_mapper.Map<ProjectResponse>)
            .ToArray();
    }
}