using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.States.UpdateState;

internal sealed class UpdateStateCommandHandler
    : ICommandHandler<UpdateStateCommand>
{
    private readonly IStateRepository _stateRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateStateCommandHandler(
        IStateRepository stateRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _stateRepository = stateRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(
        UpdateStateCommand command,
        CancellationToken cancellationToken)
    {
        var stateEntity = await _stateRepository
            .GetByIdAsync(command.StateId);

        if (stateEntity is null)
        {
            return Result
                .Failure(StateErrors.NotFound);
        }

        stateEntity.Name = command.Name;
        stateEntity.Description = command.Description;
        stateEntity.Number = command.Number;
        stateEntity.UpdatedBy = command.UserId;
        stateEntity.UpdatedAt = _dateTimeProvider.GetCurrentTime();

        await _stateRepository
            .UpdateAsync(stateEntity);

        return Result.Success();
    }
}
