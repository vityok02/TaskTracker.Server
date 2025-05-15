using Application.Abstract.Messaging;

namespace Application.Modules.Tasks.RemoveTag;

public sealed record RemoveTagCommand(
    Guid TaskId,
    Guid TagId,
    Guid UserId)
    : ICommand;
