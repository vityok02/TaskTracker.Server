namespace Domain;

public class Comment : AuditableEntity
{
    public string Name { get; private set; }
    public AppTask Task { get; private set; }

    private Comment(string name, AppTask task)
    {
        Name = name;
        Task = task;
    }

    public static Comment Create(string content, AppTask task)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);
        return new Comment(content, task);
    }
}
