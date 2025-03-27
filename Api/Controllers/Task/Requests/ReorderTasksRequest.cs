namespace Api.Controllers.Task.Requests;

public record ReorderTasksRequest(
    Guid? BeforeTaskId);
