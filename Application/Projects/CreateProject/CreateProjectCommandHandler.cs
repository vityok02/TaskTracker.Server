using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Application.Users;
using Domain.Abstract;
using Domain.Entities;

namespace Application.Projects.CreateProject;

internal sealed class CreateProjectCommandHandler
    : ICommandHandler<CreateProjectCommand, ProjectResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IDateTimeService _dateTimeService;

    public CreateProjectCommandHandler(
        IUserRepository userRepository,
        IProjectRepository projectRepository,
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
        var user = await _userRepository
            .GetByIdAsync(command.UserId);

        if (user is null)
        {
            return Result<ProjectResponse>
                .Failure(UserErrors.UserNotFound);
        }

        bool projectExists = await _projectRepository
            .ExistsAsync(command.UserId, command.Project.Name);

        if (projectExists)
        {
            return Result<ProjectResponse>
                .Failure(UserErrors.ProjectAlreadyExists(command.UserId, command.Project.Name));
        }

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = command.Project.Name,
            Description = command.Project.Description,
            CreatedAt = _dateTimeService.GetCurrentTime(),
            CreatedBy = command.UserId
        };

        var projectId = await _projectRepository
            .CreateAsync(command.UserId, project);

        return Result<ProjectResponse>
            .Success(new (projectId, project.Name, project.Description));
    }
}
