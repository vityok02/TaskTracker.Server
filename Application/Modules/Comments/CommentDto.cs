using Application.Dtos;

namespace Application.Modules.Comments;

public class CommentDto
{
    public Guid Id { get; init; }
    public string Comment { get; init; } = string.Empty;

    public Guid TaskId { get; init; }
    public string TaskName { get; init; } = string.Empty;

    public UserInfoDto CreatedBy { get; init; } = new();
    public DateTime CreatedAt { get; init; }

    public UserInfoDto? UpdatedBy { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
