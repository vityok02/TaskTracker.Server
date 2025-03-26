using Application.Abstract.Messaging;

namespace Application.Modules.Tasks.ReorderTasks;

public sealed record ReorderTasksCommand(
    Guid TaskId,
    Guid? BeforeTaskId,
    Guid ProjectId,
    Guid UserId)
    : ICommand;
