using Domain.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Templates;

[Table("TemplateTag")]
public class TemplateTagEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = null!;

    public int SortOrder { get; set; }

    public Guid TemplateId { get; set; }
}