namespace Domain.Models;

public class StateModel : AuditableModel
{
    public Guid Id { get; set; }

    public int Number { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}
