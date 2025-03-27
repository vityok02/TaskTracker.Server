namespace Application.Modules.Projects;

public class ProjectStateDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int SortOrder { get; set; }
}
