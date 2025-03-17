using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.States.GetStateById;

internal sealed class GetStateByIdQueryHandler
    : IQueryHandler<GetStateByIdQuery, StateDto>
{
    private readonly IStateRepository _stateRepository;
    private readonly IMapper _mapper;

    public GetStateByIdQueryHandler(IStateRepository stateRepository, IMapper mapper)
    {
        _stateRepository = stateRepository;
        _mapper = mapper;
    }

    public async Task<Result<StateDto>> Handle(
        GetStateByIdQuery command,
        CancellationToken cancellationToken)
    {
        var state = await _stateRepository
            .GetExtendedByIdAsync(command.Id);

        if (state is null)
        {
            return Result<StateDto>
                .Failure(StateErrors.NotFound);
        }

        return Result<StateDto>
            .Success(_mapper.Map<StateDto>(state));
    }
}
