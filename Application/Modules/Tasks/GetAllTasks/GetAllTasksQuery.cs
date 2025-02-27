using Application.Abstract.Messaging;

namespace Application.Modules.Tasks.GetAllTasks;

public sealed record GetAllTasksQuery(Guid ProjectId)
    : IQuery<IEnumerable<TaskDto>>;
