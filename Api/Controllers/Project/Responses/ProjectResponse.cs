using Api.Controllers.Abstract;
using Api.Controllers.State.Responses;

namespace Api.Controllers.Project.Responses;

public class ProjectResponse : AuditableResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public IEnumerable<StateResponse> States { get; init; } = [];
}
