using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.States.CreateState;

internal sealed class CreateStateCommandHandler
    : ICommandHandler<CreateStateCommand, StateDto>
{
    private readonly IStateRepository _stateRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public CreateStateCommandHandler(
        IStateRepository stateRepository,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper)
    {
        _stateRepository = stateRepository;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<Result<StateDto>> Handle(
        CreateStateCommand command,
        CancellationToken cancellationToken)
    {
        var lastOrder = await _stateRepository
            .GetLastOrderAsync(command.ProjectId);

        var stateEntity = new StateEntity
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Color = command.Color ?? DefaultStateColor.Value,
            SortOrder = lastOrder + 1,
            ProjectId = command.ProjectId,
            CreatedBy = command.UserId,
            CreatedAt = _dateTimeProvider.GetCurrentTime()
        };

        var stateId = await _stateRepository
            .CreateAsync(stateEntity);

        var stateModel = await _stateRepository
            .GetExtendedByIdAsync(stateId);

        if (stateModel is null)
        {
            return Result<StateDto>
                .Failure(StateErrors.NotFound);
        }

        return Result<StateDto>
            .Success(_mapper.Map<StateDto>(stateModel));
    }
}
