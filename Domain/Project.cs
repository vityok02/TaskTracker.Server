using Domain.Abstract;

namespace Domain;

public class Project : BaseEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public ICollection<User> Users { get; private set; }
    public ICollection<AppTask> Tasks { get; private set; }

    private Project(
        string name,
        string? description,
        DateTime startDate,
        DateTime? endDate,
        ICollection<User> users,
        ICollection<AppTask> tasks)
    {
        Name = name;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Users = users;
        Tasks = tasks;
    }

    public static Project Create(
        string name,
        string? description,
        DateTime startDate,
        DateTime? endDate,
        ICollection<User> users,
        ICollection<AppTask> tasks)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (startDate > endDate)
        {
            throw new ArgumentException("Start date must be before end date.");
        }

        return new Project(name, description, startDate, endDate, users, tasks);
    }
}
