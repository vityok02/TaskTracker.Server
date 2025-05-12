using Api.Controllers.Abstract;
using Api.Controllers.Tag.Responses;

namespace Api.Controllers.Task.Responses;

public class TaskResponse : AuditableResponse
{
    public Guid Id {get; init;}
    public string Name { get; init; } = string.Empty;
    public string? Description {get; init; }
    public int SortOrder { get; init; }

    public Guid ProjectId {get; init;}
    public string ProjectName { get; init; } = string.Empty;

    public Guid StateId {get; init;}
    public string StateName { get; init; } = string.Empty;
    public string StateColor { get; init; } = string.Empty;

    public IEnumerable<TagResponse> Tags { get; init; } = [];

    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}