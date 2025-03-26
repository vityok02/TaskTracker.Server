namespace Domain.Models;

public class ProjectStateModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int SortOrder { get; set; }
}