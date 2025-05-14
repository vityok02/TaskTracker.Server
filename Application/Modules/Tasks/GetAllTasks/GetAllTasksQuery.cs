using Application.Abstract.Messaging;

namespace Application.Modules.Tasks.GetAllTasks;

public sealed record GetAllTasksQuery(
    Guid ProjectId,
    string? SearchTerm,
    IEnumerable<Guid>? TagIds)
    : IQuery<IEnumerable<TaskDto>>;
