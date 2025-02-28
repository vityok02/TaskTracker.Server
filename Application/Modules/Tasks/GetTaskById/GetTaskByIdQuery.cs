using Application.Abstract.Messaging;

namespace Application.Modules.Tasks.GetTaskById;

public sealed record GetTaskByIdQuery(Guid ProjectId, Guid TaskId)
    : IQuery<TaskDto>;
