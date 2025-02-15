using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Shared;

namespace Application.Modules.Projects.GetAllProjects;

internal sealed class GetAllProjectsQueryHandler
    : IQueryHandler<GetAllProjectsQuery, IEnumerable<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;

    public GetAllProjectsQueryHandler(
        IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Result<IEnumerable<ProjectDto>>> Handle(
        GetAllProjectsQuery query,
        CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAllAsync(query.MemberId);

        return Result<IEnumerable<ProjectDto>>
            .Success(projects);
    }
}