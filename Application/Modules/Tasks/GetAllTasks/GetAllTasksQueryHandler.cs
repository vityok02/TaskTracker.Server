using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Shared;

namespace Application.Modules.Tasks.GetAllTasks;

internal sealed class GetAllTasksQueryHandler
    : IQueryHandler<GetAllTasksQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public GetAllTasksQueryHandler(
        ITaskRepository taskRepository,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<TaskDto>>> Handle(
        GetAllTasksQuery query,
        CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository
            .GetAllExtendedAsync(query.ProjectId, query.SearchTerm);

        return Result<IEnumerable<TaskDto>>
            .Success(_mapper.Map<IEnumerable<TaskDto>>(tasks));
    }
}
