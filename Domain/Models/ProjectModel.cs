namespace Domain.Models;

public class ProjectModel : AuditableModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public List<StateModel> States { get; set; } = [];
}
