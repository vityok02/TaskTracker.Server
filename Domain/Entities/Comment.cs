namespace Domain.Entities;

public class Comment : AuditableEntity
{
    public string Name { get; set; }
    public Guid TaskId { get; set; }
}
