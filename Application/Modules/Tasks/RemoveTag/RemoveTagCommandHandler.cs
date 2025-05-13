using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Tasks.RemoveTag;

internal sealed class RemoveTagCommandHandler
    : ICommandHandler<RemoveTagCommand>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITagRepository _tagRepository;

    public RemoveTagCommandHandler(
        ITaskRepository taskRepository,
        IDateTimeProvider dateTimeProvider,
        ITagRepository tagRepository)
    {
        _taskRepository = taskRepository;
        _dateTimeProvider = dateTimeProvider;
        _tagRepository = tagRepository;
    }

    public async Task<Result> Handle(
        RemoveTagCommand command,
        CancellationToken cancellationToken)
    {
        var taskTask = _taskRepository
            .GetByIdAsync(command.TaskId);

        var hasTagTask = _taskRepository
            .HasTagAsync(command.TaskId, command.TagId);

        await Task.WhenAll(taskTask, hasTagTask);

        var task = taskTask.Result;
        var hasTag = hasTagTask.Result;

        if (task is null)
        {
            return Result
                .Failure(TaskErrors.NotFound);
        }

        if (!hasTag)
        {
            return Result
                .Failure(TaskErrors.TagNotFound);
        }

        await _taskRepository
            .RemoveTagAsync(command.TaskId, command.TagId);

        task.UpdatedBy = command.UserId;
        task.UpdatedAt = _dateTimeProvider
            .GetCurrentTime();

        return Result
            .Success();
    }
}
