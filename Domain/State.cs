using Domain.Abstract;

namespace Domain;

public class State : AuditableEntity
{
    public int Number { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    private State(string name)
    {
        Name = name;
    }

    public static State Create(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new State(name);
    }
}
