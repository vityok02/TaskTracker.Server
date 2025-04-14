using Application.Abstract.Messaging;

namespace Application.Modules.Tasks.PartialUpdateTask;

public sealed record PartialUpdateTaskCommand(
    Guid TaskId,
    string? Name,
    string? Description,
    DateTime? StartDate,
    DateTime? EndDate,
    Guid? StateId,
    Guid UserId,
    Guid ProjectId)
    : ICommand;
