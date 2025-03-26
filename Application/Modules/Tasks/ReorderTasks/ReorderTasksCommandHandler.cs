using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Application.Extensions;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Tasks.ReorderTasks;

internal sealed record ReorderTasksCommandHandler
    : ICommandHandler<ReorderTasksCommand>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ReorderTasksCommandHandler(
        ITaskRepository taskRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _taskRepository = taskRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(
        ReorderTasksCommand command,
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
            .GetAllByStateId(task.StateId))
            .ToList();

        tasks.RemoveAll(t => t.Id == command.TaskId);

        task.UpdatedBy = command.UserId;
        task.UpdatedAt = DateTime.UtcNow;

        tasks.InsertInOrderedList(command.BeforeTaskId, task);
        tasks.Reorder();

        await _taskRepository
            .UpdateRangeAsync(tasks);

        return Result.Success();
    }
}
