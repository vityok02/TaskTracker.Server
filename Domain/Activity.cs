namespace Domain;

public class Activity : AuditableEntity
{
    public string Name { get; private set; }
    public string Details { get; private set; }
    public User user { get; private set; }
    public Project Project { get; private set; }
}
