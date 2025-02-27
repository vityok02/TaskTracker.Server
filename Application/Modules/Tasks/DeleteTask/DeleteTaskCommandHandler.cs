using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Tasks.DeleteTask;

internal sealed class DeleteTaskCommandHandler
    : ICommandHandler<DeleteTaskCommand>
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result> Handle(
        DeleteTaskCommand command,
        CancellationToken cancellationToken)
    {
        var task = await _taskRepository
            .GetByIdAsync(command.TaskId);

        if (task?.ProjectId != command.ProjectId)
        {
            return Result
                .Failure(TaskErrors.NotFound);
        }

        await _taskRepository
            .DeleteAsync(command.TaskId);

        return Result
            .Success();
    }
}