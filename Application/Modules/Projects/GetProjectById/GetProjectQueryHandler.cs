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
    private readonly IMapper _mapper;

    public GetProjectQueryHandler(
        IProjectRepository projectRepository,
        IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<Result<ProjectDto>> Handle(
        GetProjectQuery query,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository
            .GetExtendedByIdAsync(query.ProjectId);

        if (project is null)
        {
            return Result<ProjectDto>
                .Failure(ProjectErrors.NotFound);
        }

        return _mapper.Map<ProjectDto>(project);
    }
}