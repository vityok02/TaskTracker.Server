using Application.Abstract.Messaging;

namespace Application.Modules.States.CreateState;

public record CreateStateCommand(
    string Name,
    string? Description,
    Guid ProjectId,
    Guid UserId)
    : ICommand<StateDto>;
