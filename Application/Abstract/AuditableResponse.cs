namespace Application.Abstract;

public record AuditableResponse(
    Guid CreatedBy,
    DateTime CreatedAt,
    Guid? UpdatedBy,
    DateTime? UpdatedAt);
