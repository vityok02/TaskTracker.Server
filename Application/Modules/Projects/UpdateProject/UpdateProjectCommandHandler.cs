using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Projects.UpdateProject;

internal sealed class UpdateProjectCommandHandler
    : ICommandHandler<UpdateProjectCommand>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateProjectCommandHandler(
        IProjectRepository projectRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _projectRepository = projectRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(
        UpdateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository
            .GetByIdAsync(command.ProjectId);

        var exists = await _projectRepository
            .ExistsByNameAsync(command.UserId, command.Name);

        if (project is null)
        {
            return Result
                .Failure(ProjectErrors.NotFound);
        }

        if (exists && !string.Equals(
            project.Name,
            command.Name,
            StringComparison.OrdinalIgnoreCase))
        {
            return Result
                .Failure(ProjectErrors.AlreadyExists);
        }

        project.Name = command.Name;
        project.Description = command.Description;
        project.StartDate = command.StartDate;
        project.EndDate = command.EndDate;
        project.UpdatedAt = _dateTimeProvider
            .GetCurrentTime();
        project.UpdatedBy = command.UserId;

        await _projectRepository
            .UpdateAsync(project);

        return Result.Success();
    }
}