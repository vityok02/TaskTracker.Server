using Api.Controllers.Abstract;

namespace Api.Controllers.Comment.Responses;

public class CommentResponse : AuditableResponse
{
    public Guid Id { get; init; }
    public string Comment { get; init; } = string.Empty;

    public Guid TaskId { get; init; }
    public string TaskName { get; init; } = string.Empty;
}
