using Application.Abstract.Messaging;

namespace Application.Modules.Tasks.UpdateTaskState;

public sealed record UpdateTaskStateCommand(
    Guid TaskId,
    Guid StateId,
    Guid? BeforeTaskId,
    Guid ProjectId,
    Guid UserId)
    : ICommand;
