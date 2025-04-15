using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Tasks.UpdateTask;

internal sealed class UpdateTaskCommandHandler
    : ICommandHandler<UpdateTaskCommand>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IStateRepository _stateRepository;

    public UpdateTaskCommandHandler(
        ITaskRepository taskRepository,
        IDateTimeProvider dateTimeProvider,
        IStateRepository stateRepository)
    {
        _taskRepository = taskRepository;
        _dateTimeProvider = dateTimeProvider;
        _stateRepository = stateRepository;
    }

    public async Task<Result> Handle(
        UpdateTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = await _taskRepository
            .GetByIdAsync(command.Id);

        if (task is null)
        {
            return Result.Failure(TaskErrors.NotFound);
        }

        var taskExists = await _taskRepository
            .ExistsByNameForProjectAsync(command.Name, command.ProjectId);

        if (taskExists && !string.Equals(
            task.Name,
            command.Name))
        {
            return Result.Failure(TaskErrors.AlreadyExists);
        }

        var stateExists = await _stateRepository
            .ExistsForProject(
            command.StateId,
            command.ProjectId);

        if (!stateExists)
        {
            return Result.Failure(TaskErrors.InvalidState);
        }

        task.Name = command.Name;
        task.Description = command.Description;
        task.StateId = command.StateId;
        task.UpdatedAt = _dateTimeProvider.GetCurrentTime();
        task.UpdatedBy = command.UserId;
        task.StartDate = command.StartDate;
        task.EndDate = command.EndDate;

        await _taskRepository
            .UpdateAsync(task);

        return Result.Success();
    }
}
