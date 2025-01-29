using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Application.Users;
using Domain;
using Domain.Abstract;

namespace Application.Projects.CreateProject;

internal sealed class CreateProjectCommandHandler
    : ICommandHandler<CreateProjectCommand, ProjectResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository<Guid> _projectRepository;
    private readonly IDateTimeService _dateTimeService;

    public CreateProjectCommandHandler(
        IUserRepository userRepository,
        IProjectRepository<Guid> projectRepository,
        IDateTimeService dateTimeService)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _dateTimeService = dateTimeService;
    }

    public async Task<Result<ProjectResponse>> Handle(
        CreateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdWithProjectsAsync(command.UserId);

        if (user is null)
        {
            return Result<ProjectResponse>.Failure(UserErrors.UserNotFound);
        }

        if (user.Projects.Any(p => p.Name == command.Project.Name))
        {
            return Result<ProjectResponse>.Failure(UserErrors.ProjectAlreadyExists);
        }

        var project = Project.Create(
            Guid.NewGuid(),
            command.Project.Name,
            command.Project.Description,
            user,
            _dateTimeService.GetCurrentTime());

        var projectId = await _projectRepository.CreateAsync(command.UserId, project);

        user.AddProject(project);

        // What happens if the user is simpy updated?

        return Result<ProjectResponse>
            .Success(new ProjectResponse(projectId, project.Name, project.Description));
    }
}
