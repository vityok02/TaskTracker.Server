using Application.Abstract;
using Application.Modules.Tags;

namespace Application.Modules.Tasks;

public class TaskDto : AuditableDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int SortOrder { get; init; }

    public Guid ProjectId { get; init; }
    public string ProjectName { get; init; } = string.Empty;

    public Guid StateId { get; init; }
    public string StateName { get; init; } = string.Empty;

    public IEnumerable<TagDto> Tags { get; init; } = new List<TagDto>();

    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}