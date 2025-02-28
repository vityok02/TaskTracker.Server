using Application.Abstract;

namespace Application.Modules.Projects;

public class ProjectDto : AuditableDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public IEnumerable<ProjectStateDto> States { get; init; } = [];
}
