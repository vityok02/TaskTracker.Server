using Application.Abstract.Messaging;

namespace Application.Modules.States.UpdateStateOrder;

public sealed record ReorderStatesCommand(
    Guid StateId,
    Guid? BeforeStateId,
    Guid ProjectId,
    Guid UserId)
    : ICommand;
