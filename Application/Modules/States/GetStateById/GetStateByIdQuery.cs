using Application.Abstract.Messaging;

namespace Application.Modules.States.GetStateById;

public sealed record GetStateByIdQuery(Guid Id)
    : IQuery<StateDto>;
