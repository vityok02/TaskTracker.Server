using Domain.Abstract;

namespace Domain.Entities;

public class State : AuditableEntity
{
    public int Number { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
