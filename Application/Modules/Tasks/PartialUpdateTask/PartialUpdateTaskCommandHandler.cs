using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Tasks.PartialUpdateTask;

internal sealed class PartialUpdateTaskCommandHandler
    : ICommandHandler<PartialUpdateTaskCommand>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IStateRepository _stateRepository;

    public PartialUpdateTaskCommandHandler(
        ITaskRepository taskRepository,
        IDateTimeProvider dateTimeProvider,
        IStateRepository stateRepository)
    {
        _taskRepository = taskRepository;
        _dateTimeProvider = dateTimeProvider;
        _stateRepository = stateRepository;
    }

    public async Task<Result> Handle(
        PartialUpdateTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = await _taskRepository
            .GetByIdAsync(command.TaskId);

        if (task is null)
        {
            return Result
                .Failure(TaskErrors.NotFound);
        }

        if (command.Name is not null)
        {
            task.Name = command.Name;
        }

        if (command.Description is not null)
        {
            task.Description = command.Description;
        }

        if (command.StartDate is not null)
        {
            task.StartDate = command.StartDate;
        }

        if (command.EndDate is not null)
        {
            task.EndDate = command.EndDate;
        }

        if (command.StateId is not null)
        {
            bool stateExist = await _stateRepository
                .ExistsForProject(command.StateId.Value, command.ProjectId);

            if (stateExist)
            {
                task.StateId = command.StateId.Value;
            }
        }

        task.UpdatedBy = command.UserId;
        task.UpdatedAt = _dateTimeProvider
            .GetCurrentTime();

        await _taskRepository
            .UpdateAsync(task);

        return Result.Success();
    }
}
