using Application.Abstract.Messaging;

namespace Application.Modules.States.UpdateStateNumbers;

public sealed record UpdateStateNumbersCommand(
    Guid StateId1,
    Guid StateId2,
    Guid ProjectId,
    Guid UserId)
    : ICommand;
