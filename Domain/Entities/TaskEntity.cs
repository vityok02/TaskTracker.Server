using Domain.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Task")]
public class TaskEntity : SortableEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public Guid StateId { get; set; }

    public Guid ProjectId { get; set; }
}
