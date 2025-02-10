namespace Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj is not BaseEntity other)
        {
            return false;
        }

        return Equals(Id, other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
