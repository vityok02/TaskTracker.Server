namespace Domain;

public class Project : AuditableEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public ICollection<User> Users { get; private set; }
    public ICollection<AppTask> Tasks { get; private set; }

    private Project(
        Guid id,
        string name,
        string? description,
        DateTime createdAt,
        Guid createdBy,
        DateTime updatedAt,
        Guid updatedBy,
        ICollection<User> users,
        ICollection<AppTask> tasks)
    {
        Id = id;
        Name = name;
        Description = description;
        Users = users;
        Tasks = tasks;
    }

    //public static Project Create(
    //    string name,
    //    string? description,
    //    ICollection<User> users,
    //    ICollection<AppTask> tasks)
    //{
    //    ArgumentException.ThrowIfNullOrWhiteSpace(name);

    //    if (startDate > endDate)
    //    {
    //        throw new ArgumentException("Start date must be before end date.");
    //    }

    //    return new Project(name, description, startDate, endDate, users, tasks);
    //}
}
