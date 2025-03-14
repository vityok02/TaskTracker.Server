using Application.Abstract.Messaging;

namespace Application.Modules.States.UpdateState;

public sealed record UpdateStateCommand(
    Guid StateId,
    string Name,
    string? Description,
    int Number,
    Guid UserId)
    : ICommand;
