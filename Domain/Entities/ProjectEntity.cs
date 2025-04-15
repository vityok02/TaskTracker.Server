using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Project")]
public class ProjectEntity : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
