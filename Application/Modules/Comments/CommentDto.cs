using Application.Abstract;

namespace Application.Modules.Comments;

public class CommentDto : AuditableDto
{
    public Guid Id { get; init; }
    public string Comment { get; init; } = string.Empty;

    public Guid TaskId { get; init; }
    public string Taskname { get; init; } = string.Empty;
}
