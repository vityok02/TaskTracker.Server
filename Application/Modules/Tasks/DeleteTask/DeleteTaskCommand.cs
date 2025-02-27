using Application.Abstract.Messaging;

namespace Application.Modules.Tasks.DeleteTask;

public sealed record DeleteTaskCommand(
    Guid TaskId,
    Guid ProjectId)
    : ICommand;
