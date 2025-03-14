using Application.Abstract.Messaging;

namespace Application.Modules.States.GetProjectStates;

public record GetProjectStatesQuery(Guid ProjectId)
    : ICommand<IEnumerable<StateDto>>;
