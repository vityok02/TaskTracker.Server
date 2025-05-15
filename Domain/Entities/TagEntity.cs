using Domain.Abstract;
using Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Tag")]
public class TagEntity : SortableEntity
{
    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = Constants.Colors.Default;

    public Guid ProjectId { get; set; }
}
