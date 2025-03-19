using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.States.UpdateStateNumbers;

internal sealed class UpdateStateNumbersCommandHandler
    : ICommandHandler<UpdateStateNumbersCommand>
{
    private readonly IStateRepository _stateRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateStateNumbersCommandHandler(
        IStateRepository stateRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _stateRepository = stateRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(
        UpdateStateNumbersCommand command,
        CancellationToken cancellationToken)
    {
        var (state1, state2) = await _stateRepository.GetBothByIdsAsync(
            command.StateId1,
            command.StateId2);

        if (state1 is null || state2 is null)
        {
            return Result.Failure(StateErrors.NotFound);
        }

        if (state1.ProjectId != command.ProjectId
            || state2.ProjectId != command.ProjectId)
        {
            return Result.Failure(StateErrors.Forbidden);
        }

        state1.UpdatedBy = command.UserId;
        state1.UpdatedAt = _dateTimeProvider
            .GetCurrentTime();

        state2.UpdatedBy = command.UserId;
        state2.UpdatedAt = _dateTimeProvider
            .GetCurrentTime();

        (state1.Number, state2.Number) = (state2.Number, state1.Number);

        await _stateRepository
            .UpdateBothAsync(state1, state2);

        return Result.Success();
    }
}