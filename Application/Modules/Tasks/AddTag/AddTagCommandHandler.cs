using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Tasks.AddTag;

internal sealed class AddTagCommandHandler
    : ICommandHandler<AddTagCommand, TaskDto>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public AddTagCommandHandler(
        ITaskRepository taskRepository,
        ITagRepository tagRepository,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _tagRepository = tagRepository;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<Result<TaskDto>> Handle(
        AddTagCommand command,
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
            return Result<TaskDto>
                .Failure(TaskErrors.NotFound);
        }

        if (tag is null)
        {
            return Result<TaskDto>
                .Failure(TagErrors.NotFound);
        }

        await _taskRepository
            .AddTagAsync(command.TaskId, command.TagId);

        task.UpdatedBy = command.UserId;

        task.UpdatedAt = _dateTimeProvider
            .GetCurrentTime();

        await _taskRepository
            .UpdateAsync(task);

        return Result<TaskDto>
            .Success(_mapper.Map<TaskDto>(task));
    }
}
