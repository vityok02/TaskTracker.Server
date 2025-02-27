using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Task")]
public class AppTask : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid StateId { get; set; }

    public Guid ProjectId { get; set; }
}
