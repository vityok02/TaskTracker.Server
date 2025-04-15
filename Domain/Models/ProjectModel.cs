namespace Domain.Models;

public class ProjectModel : AuditableModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public List<StateModel> States { get; set; } = [];
}
