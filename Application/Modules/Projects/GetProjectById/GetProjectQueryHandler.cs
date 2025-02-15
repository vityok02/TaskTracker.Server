using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Projects.GetProjectById;

internal sealed class GetProjectQueryHandler
    : IQueryHandler<GetProjectQuery, ProjectDto>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IMapper _mapper;

    public GetProjectQueryHandler(
        IProjectRepository projectRepository,
        IMapper mapper,
        IProjectMemberRepository projectMemberRepository)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _projectMemberRepository = projectMemberRepository;
    }

    public async Task<Result<ProjectDto>> Handle(
        GetProjectQuery query,
        CancellationToken cancellationToken)
    {
        var member = await _projectMemberRepository
            .GetAsync(query.UserId, query.ProjectId);

        if (member is null)
        {
            return Result<ProjectDto>
                .Failure(ProjectMemberErrors.NotFound);
        }

        var project = await _projectRepository
            .GetByIdAsync(query.UserId, query.ProjectId);

        return _mapper.Map<ProjectDto>(project);
    }
}