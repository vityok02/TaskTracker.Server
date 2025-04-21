using Domain.Models.Common;

namespace Domain.Models;

public class CommentModel
{
    public Guid Id { get; set; }
    public string Comment { get; set; } = string.Empty;

    public UserInfoModel CreatedBy { get; set; } = new();
    public DateTime CreatedAt { get; set; }

    public UserInfoModel? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Guid TaskId { get; set; }
    public string TaskName { get; set; } = string.Empty;
}
