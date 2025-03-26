namespace Api.Controllers.Task.Requests;

public record UpdateTaskStateRequest(
    Guid StateId,
    Guid? BeforeTaskId);
