namespace Domain;

public class Project : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public ICollection<User> Users { get; private set; } = [];
    public ICollection<AppTask> Tasks { get; private set; } = [];

    private Project(
        Guid id,
        string name,
        string? description,
        User user)
    {
        Id = id;
        Name = name;
        Description = description;
        Users.Add(user);
    }

    private Project()
    { }

    public static Project Create(
        Guid id,
        string name,
        string? description,
        User user,
        DateTime createdAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var project = new Project(id, name, description, user);
        project.Create(user.Id, createdAt);
        return project;
    }

    public void Update(
        string name,
        string? description,
        User user,
        DateTime updatedAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
        Description = description;
        Update(user.Id, updatedAt);
    }

    public void AddUser(User user)
    {
        if (Users.Any(u => u.Equals(user)))
        {
            throw new ArgumentException("User already exists in project.");
        }

        Users.Add(user);
    }

    public void AddTask(AppTask task)
    {
        if (Tasks.Any(t => t.Equals(task)))
        {
            throw new ArgumentException("Task already exists in project.");
        }

        Tasks.Add(task);
    }
}
