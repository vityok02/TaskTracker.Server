using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Tasks.GetTaskById;

public sealed record GetTaskByIdQuery(Guid ProjectId, Guid TaskId)
    : IQuery<TaskDto>;

internal sealed class GetTaskByIdQueryHandler
    : IQueryHandler<GetTaskByIdQuery, TaskDto>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public GetTaskByIdQueryHandler(
        ITaskRepository taskRepository,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<Result<TaskDto>> Handle(
        GetTaskByIdQuery query,
        CancellationToken cancellationToken)
    {
        var task = await _taskRepository
            .GetByIdAsync(query.TaskId);

        if (task?.ProjectId != query.ProjectId)
        {
            return Result<TaskDto>
                .Failure(TaskErrors.NotFound);
        }

        return Result<TaskDto>
            .Success(_mapper.Map<TaskDto>(task));
    }
}
