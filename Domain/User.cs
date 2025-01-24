using Domain.Abstract;

namespace Domain;

public class User : BaseEntity
{
    public string UserName { get; private set; }
    public ICollection<Project> Projects { get; private set; } = [];

    private User(Guid id, string userName)
    {
        Id = id;
        UserName = userName;
    }

    public static User Create(Guid id, string userName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);

        return new User(id, userName);
    }
}
