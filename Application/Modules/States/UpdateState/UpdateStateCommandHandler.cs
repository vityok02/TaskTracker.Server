using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.States.UpdateState;

internal sealed class UpdateStateCommandHandler
    : ICommandHandler<UpdateStateCommand, StateDto>
{
    private readonly IStateRepository _stateRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public UpdateStateCommandHandler(
        IStateRepository stateRepository,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper)
    {
        _stateRepository = stateRepository;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<Result<StateDto>> Handle(
        UpdateStateCommand command,
        CancellationToken cancellationToken)
    {
        var stateEntity = await _stateRepository
            .GetByIdAsync(command.StateId);

        if (stateEntity is null)
        {
            return Result<StateDto>
                .Failure(StateErrors.NotFound);
        }

        if (stateEntity.ProjectId != command.ProjectId)
        {
            return Result<StateDto>
                .Failure(StateErrors.Forbidden);
        }

        stateEntity.Name = command.Name;
        stateEntity.Description = command.Description;

        stateEntity.Color = string.IsNullOrWhiteSpace(command.Color)
            ? DefaultColor.Value : command.Color;

        stateEntity.UpdatedBy = command.UserId;
        stateEntity.UpdatedAt = _dateTimeProvider.GetCurrentTime();

        await _stateRepository
            .UpdateAsync(stateEntity);

        return Result<StateDto>
            .Success(_mapper.Map<StateDto>(stateEntity));
    }
}
