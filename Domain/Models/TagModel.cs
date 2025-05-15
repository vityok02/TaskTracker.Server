namespace Domain.Models;

public class TagModel : AuditableModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}
