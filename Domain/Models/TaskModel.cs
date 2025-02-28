namespace Domain.Models;

public class TaskModel : AuditableModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid ProjectId { get; set; }

    public string ProjectName { get; set; } = string.Empty;

    public Guid StateId { get; set; }

    public string State { get; set; } = string.Empty;
}
