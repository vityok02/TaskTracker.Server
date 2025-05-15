using Domain.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Templates;

[Table("Template")]
public class TemplateEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int SortOrder { get; set; }
}
