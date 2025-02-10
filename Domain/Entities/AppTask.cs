namespace Domain.Entities;

public class AppTask : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid StateId { get; set; }

    public Guid ProjectId { get; set; }
}
