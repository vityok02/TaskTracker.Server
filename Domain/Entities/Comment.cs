namespace Domain.Entities;

public class Comment : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public Guid TaskId { get; set; }
}
