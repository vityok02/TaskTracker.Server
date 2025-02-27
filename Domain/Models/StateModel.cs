namespace Domain.Models;

public class StateModel : AuditableModel
{
    public int Number { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;
}
