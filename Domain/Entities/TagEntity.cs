using Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Tag")]
public class TagEntity : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = DefaultColor.Value;

    public int SortOrder { get; set; }

    public Guid ProjectId { get; set; }
}
