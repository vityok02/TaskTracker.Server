using Application.Abstract.Messaging;

namespace Application.Modules.Tasks.AddTag;

public sealed record AddTagCommand(
    Guid TaskId,
    Guid TagId,
    Guid UserId)
    : ICommand<TaskDto>;
