using Application.Abstract.Messaging;

namespace Application.Modules.States.UpdateStateOrders;

public sealed record ReorderStatesCommand(
    Guid StateId,
    Guid? BeforeStateId,
    Guid ProjectId,
    Guid UserId)
    : ICommand;
