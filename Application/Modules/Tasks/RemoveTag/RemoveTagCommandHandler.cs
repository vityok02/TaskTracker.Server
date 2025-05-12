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
        var tagTask = _tagRepository
            .GetByIdAsync(command.TagId);

        await Task.WhenAll(taskTask, tagTask);

        var task = taskTask.Result;
        var tag = tagTask.Result;

        if (task is null)
        {
            return Result
                .Failure(TaskErrors.NotFound);
        }

        if (tag is null)
        {
            return Result
                .Failure(TagErrors.NotFound);
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
