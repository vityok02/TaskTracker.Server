using Domain.Abstract;
using Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("State")]
public class StateEntity : SortableEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;

    public string Color { get; set;} = Colors.Default;

    public Guid ProjectId { get; set; }
}
