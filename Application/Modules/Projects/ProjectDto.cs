using Application.Abstract;
using Application.Modules.States;

namespace Application.Modules.Projects;

public class ProjectDto : AuditableDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public DateTime? StartDate { get; init; }

    public DateTime? EndDate { get; init; }

    public IEnumerable<StateDto> States { get; init; } = [];
}
