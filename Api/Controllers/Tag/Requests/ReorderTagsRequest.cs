namespace Api.Controllers.Tag.Requests;

public record ReorderTagsRequest(
    Guid? BeforeTagId);