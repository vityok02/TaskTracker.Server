using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Shared;

namespace Application.Modules.States.GetProjectStates;

internal sealed class GetProjectStatesQueryHandler
    : ICommandHandler<GetProjectStatesQuery, IEnumerable<StateDto>>
{
    private readonly IStateRepository _stateRepository;
    private readonly IMapper _mapper;

    public GetProjectStatesQueryHandler(
        IStateRepository stateRepository,
        IMapper mapper)
    {
        _stateRepository = stateRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<StateDto>>> Handle(
        GetProjectStatesQuery request,
        CancellationToken cancellationToken)
    {
        var states = await _stateRepository
            .GetAllByProjectIdAsync(request.ProjectId);

        return Result<IEnumerable<StateDto>>
            .Success(_mapper.Map<IEnumerable<StateDto>>(states));
    }
}
