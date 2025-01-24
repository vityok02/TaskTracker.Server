using Domain.Abstract;

namespace Domain;

public class Comment : BaseEntity
{
    public string Content { get; private set; }
    public AppTask Task { get; private set; }

    private Comment(string content, AppTask task)
    {
        Content = content;
        Task = task;
    }

    public static Comment Create(string content, AppTask task)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);
        return new Comment(content, task);
    }
}
