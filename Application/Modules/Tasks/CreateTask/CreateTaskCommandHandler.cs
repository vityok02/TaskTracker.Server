using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Models;
using Domain.Shared;

namespace Application.Modules.Tasks.CreateTask;

internal sealed class CreateTaskCommandHandler
    : ICommandHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public CreateTaskCommandHandler(
        ITaskRepository taskRepository,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<Result<TaskDto>> Handle(
        CreateTaskCommand command,
        CancellationToken cancellationToken)
    {
        var taskExists = await _taskRepository
            .ExistsByNameForProjectAsync(command.Name, command.ProjectId);

        if (taskExists)
        {
            return Result<TaskDto>
                .Failure(TaskErrors.AlreadyExists);
        }

        AppTask task = new()
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            CreatedBy = command.UserId,
            CreatedAt = _dateTimeProvider.GetCurrentTime(),
            StateId = command.StateId,
            ProjectId = command.ProjectId
        };

        TaskModel createdTask = await _taskRepository
            .CreateAsync(task);

        return Result<TaskDto>
            .Success(_mapper.Map<TaskDto>(createdTask));
    }
}