namespace Domain.Models;

public class CommentModel : AuditableModel
{
    public Guid Id { get; set; }
    public string Comment { get; set; } = string.Empty;

    public Guid TaskId { get; set; }
    public string TaskName { get; set; } = string.Empty;
}
