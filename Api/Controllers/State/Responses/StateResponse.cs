using Api.Controllers.Abstract;
using Domain.Constants;

namespace Api.Controllers.State.Responses;

public class StateResponse : AuditableResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public string? Color { get; init; } = DefaultColor.Value;

    public int SortOrder { get; init; }
}
