using Domain.Constants;

namespace Domain.Models;

public class StateModel : AuditableModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Color { get; set; } = DefaultStateColor.Value;

    public int SortOrder { get; set; }
}
