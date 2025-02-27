using Application.Abstract.Messaging;

namespace Application.Modules.Tasks.UpdateTask;

public sealed record UpdateTaskCommand(
    Guid Id,
    Guid ProjectId,
    Guid UserId,
    Guid StateId,
    string Name,
    string? Description)
    : ICommand;
