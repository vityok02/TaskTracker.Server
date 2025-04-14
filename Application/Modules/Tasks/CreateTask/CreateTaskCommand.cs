using Application.Abstract.Messaging;

namespace Application.Modules.Tasks.CreateTask;

public sealed record CreateTaskCommand(
    string Name,
    string? Description,
    DateTime? StartDate,
    Guid UserId,
    Guid ProjectId,
    Guid StateId)
    : ICommand<TaskDto>;
