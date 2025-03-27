using Api.Controllers.Abstract;

namespace Api.Controllers.State.Responses;

public class StateResponse : AuditableResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int SortOrder { get; init; }
}
