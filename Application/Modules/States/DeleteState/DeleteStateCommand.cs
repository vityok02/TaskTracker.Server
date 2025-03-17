using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.States.DeleteState;

public sealed record DeleteStateCommand(
    Guid StateId)
    : ICommand;

internal sealed class DeleteStateCommandHandler
    : ICommandHandler<DeleteStateCommand>
{
    private readonly IStateRepository _stateRepository;

    public DeleteStateCommandHandler(IStateRepository stateRepository)
    {
        _stateRepository = stateRepository;
    }

    public async Task<Result> Handle(
        DeleteStateCommand command,
        CancellationToken cancellationToken)
    {
        var stateEntity = await _stateRepository
            .GetByIdAsync(command.StateId);

        if (stateEntity is null)
        {
            return Result
                .Failure(StateErrors.NotFound);
        }

        await _stateRepository
            .DeleteAsync(command.StateId);

        return Result.Success();
    }
}
