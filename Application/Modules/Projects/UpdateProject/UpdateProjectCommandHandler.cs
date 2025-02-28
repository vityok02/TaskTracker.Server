using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Entities;
using Domain.Errors;
using Domain.Models;
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
        var projectModel = await _projectRepository
            .GetModelByUserIdAndProjectIdAsync(command.UserId, command.ProjectId);

        var exists = await _projectRepository
            .ExistsByNameAsync(command.UserId, command.Name);

        if (projectModel is null)
        {
            return Result
                .Failure(ProjectErrors.NotFound);
        }

        if (exists && !string.Equals(
            projectModel.Name,
            command.Name,
            StringComparison.OrdinalIgnoreCase))
        {
            return Result
                .Failure(ProjectErrors.AlreadyExists);
        }

        var updatedProject = new Project
        {
            Id = projectModel.Id,
            Name = command.Name,
            Description = command.Description,
            CreatedBy = projectModel.CreatedBy,
            CreatedAt = projectModel.CreatedAt,
            UpdatedBy = command.UserId,
            UpdatedAt = _dateTimeProvider.GetCurrentTime(),
        };

        await _projectRepository
            .UpdateAsync(updatedProject);

        return Result.Success();
    }
}