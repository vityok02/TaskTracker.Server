using Api.Controllers.Abstract;

namespace Api.Controllers.Project.Responses;

public class ProjectResponse : AuditableResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public IEnumerable<ProjectStateResponse> States { get; init; } = [];
}
