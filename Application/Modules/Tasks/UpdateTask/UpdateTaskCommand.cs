using Application.Abstract.Messaging;

namespace Application.Modules.Tasks.UpdateTask;

public sealed record UpdateTaskCommand(
    Guid Id,
    string Name,
    string? Description,
    DateTime? StartDate,
    DateTime? EndDate,
    Guid UserId,
    Guid ProjectId,
    Guid StateId)
    : ICommand;
