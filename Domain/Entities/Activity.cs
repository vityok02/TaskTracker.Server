namespace Domain.Entities;

public class Activity : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public string Details { get; set; } = string.Empty;

    public Guid ProjectId { get; set; }
}
