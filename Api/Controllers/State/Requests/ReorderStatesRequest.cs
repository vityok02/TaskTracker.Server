namespace Api.Controllers.State.Requests;

public record ReorderStatesRequest(
    Guid? BeforeStateId
);
