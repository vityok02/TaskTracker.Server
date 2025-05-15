using Application.Abstract;

namespace Application.Modules.Tags;

public class TagDto : AuditableDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Color { get; init; } = string.Empty;

    public int SortOrder { get; init; }

    public Guid ProjectId { get; init; }
}
