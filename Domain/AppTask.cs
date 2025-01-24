using Domain.Abstract;

namespace Domain;

public class AppTask : BaseEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public State State { get; private set; }
    public Project Project { get; private set; }

    private AppTask(string name, string? description, State state, Project project)
    {
        Name = name;
        Description = description;
        State = state;
        Project = project;
    }

    public static AppTask Create(string name, string? description, State state, Project project)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new AppTask(name, description, state, project);
    }
}
