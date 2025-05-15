using Domain.Abstract;
using Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Templates;

[Table("TemplateState")]
public class TemplateStateEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Color { get; set; } = Colors.Default;

    public int SortOrder { get; set; }

    public Guid TemplateId { get; set; }
}
