using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Application.Extensions;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Tasks.UpdateTaskState;

internal sealed class UpdateTaskStateCommandHandler
    : ICommandHandler<UpdateTaskStateCommand>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateTaskStateCommandHandler(
        ITaskRepository taskRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _taskRepository = taskRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(
        UpdateTaskStateCommand command,
        CancellationToken cancellationToken)
    {
        var task = await _taskRepository
            .GetByIdAsync(command.TaskId);

        if (task is null)
        {
            return Result
                .Failure(TaskErrors.NotFound);
        }

        if (task.ProjectId != command.ProjectId)
        {
            return Result
                .Failure(TaskErrors.Forbidden);
        }


        var tasks = (await _taskRepository
            .GetAllByStateId(command.StateId))
            .ToList();

        tasks.RemoveAll(t => t.Id == command.TaskId);

        task.StateId = command.StateId;
        task.UpdatedBy = command.UserId;
        task.UpdatedAt = _dateTimeProvider.GetCurrentTime();

        tasks.InsertInOrderedList(command.BeforeTaskId, task);
        tasks.Reorder();

        await _taskRepository.UpdateRangeAsync(tasks);

        return Result.Success();
    }
}
