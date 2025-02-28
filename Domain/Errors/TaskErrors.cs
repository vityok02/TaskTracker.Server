using Domain.Shared;

namespace Domain.Errors;

public static class TaskErrors
{
    public static Error NotFound
        => new("Task.NotFound", "Task not found.");

    public static Error AlreadyExists
        => new("Task.AlreadyExists", "Task with such name already exists.");

    public static Error InvalidState
        => new("Task.ValidationError", "The specified state does not exist or does not belong to the project.");
}
