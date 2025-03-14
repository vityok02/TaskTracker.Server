using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Comment")]
public class CommentEntity : AuditableEntity
{
    public string Comment { get; set; } = string.Empty;

    public Guid TaskId { get; set; }
}
