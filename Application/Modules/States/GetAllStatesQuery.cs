using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Shared;

namespace Application.Modules.States;

public record GetAllStatesQuery(Guid ProjectId)
    : ICommand<IEnumerable<StateDto>>;

internal sealed class GetAllStatesQueryHandler
    : ICommandHandler<GetAllStatesQuery, IEnumerable<StateDto>>
{
    private readonly IStateRepository _stateRepository;
    private readonly IMapper _mapper;

    public GetAllStatesQueryHandler(
        IStateRepository stateRepository,
        IMapper mapper)
    {
        _stateRepository = stateRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<StateDto>>> Handle(
        GetAllStatesQuery request,
        CancellationToken cancellationToken)
    {
        var states = await _stateRepository
            .GetAllByProjectIdAsync(request.ProjectId);

        return Result<IEnumerable<StateDto>>
            .Success(_mapper.Map<IEnumerable<StateDto>>(states));
    }
}
