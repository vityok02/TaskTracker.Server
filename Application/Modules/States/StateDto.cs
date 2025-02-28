using Application.Abstract;

namespace Application.Modules.States;

public class StateDto : AuditableDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public int Number { get; init; }
}
