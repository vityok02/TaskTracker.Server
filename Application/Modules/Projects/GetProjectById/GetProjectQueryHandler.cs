using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Abstract;
using Domain.Errors;

namespace Application.Modules.Projects.GetProjectById;

internal sealed class GetProjectQueryHandler
    : IQueryHandler<GetProjectQuery, ProjectResponse>
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

    public async Task<Result<ProjectResponse>> Handle(
        GetProjectQuery query,
        CancellationToken cancellationToken)
    {
        var member = await _projectMemberRepository
            .GetAsync(query.UserId, query.ProjectId);

        if (member is null)
        {
            return Result<ProjectResponse>
                .Failure(ProjectMemberErrors.NotFound);
        }

        var project = await _projectRepository
            .GetByIdAsync(query.ProjectId);

        return _mapper.Map<ProjectResponse>(project);
    }
}