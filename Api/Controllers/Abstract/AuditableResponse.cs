namespace Api.Controllers.Abstract;

public class AuditableResponse
{
    public Guid CreatedBy { get; init; }
    public string CreatedByName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }

    public Guid? UpdatedBy { get; init; }
    public string? UpdatedByName { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
