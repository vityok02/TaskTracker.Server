using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Application.Extensions;
using Application.Modules.States.UpdateStateOrders;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.States.UpdateStateNumbers;

internal sealed class ReorderStatesCommandHandler
    : ICommandHandler<ReorderStatesCommand>
{
    private readonly IStateRepository _stateRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ReorderStatesCommandHandler(
        IStateRepository stateRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _stateRepository = stateRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(
        ReorderStatesCommand command,
        CancellationToken cancellationToken)
    {
        var state = await _stateRepository
            .GetByIdAsync(command.StateId);

        if (state is null)
        {
            return Result
                .Failure(StateErrors.NotFound);
        }

        if (state.ProjectId != command.ProjectId)
        {
            return Result
                .Failure(StateErrors.Forbidden);
        }

        var states = (await _stateRepository
            .GetAllAsync(command.ProjectId))
            .OrderBy(s => s.SortOrder)
            .ToList();

        states.RemoveAll(s => s.Id == command.StateId);

        state.UpdatedBy = command.UserId;
        state.UpdatedAt = _dateTimeProvider.GetCurrentTime();

        states.InsertInOrderedList(command.BeforeStateId, state);
        states.Reorder();

        await _stateRepository.UpdateRangeAsync(states);

        return Result.Success();
    }
}