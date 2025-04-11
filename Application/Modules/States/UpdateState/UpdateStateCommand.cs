using Application.Abstract.Messaging;

namespace Application.Modules.States.UpdateState;

public sealed record UpdateStateCommand(
    Guid StateId,
    string Name,
    string? Description,
    string? Color,
    Guid ProjectId,
    Guid UserId)
    : ICommand<StateDto>;
