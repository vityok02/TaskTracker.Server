using Domain.Abstract;

namespace Domain;

public class Role : BaseEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }

    private Role(Guid id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public static Role Create(Guid id, string name, string? description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Role(id, name, description);
    }
}
