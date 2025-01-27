using Domain.Abstract;

namespace Domain;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public Guid CreatedBy { get; protected set; }
    public Guid? UpdatedBy { get; protected set; }

    public virtual void Create(Guid createdBy, DateTime createdAt)
    {
        CreatedAt = DateTime.Now;
        CreatedBy = createdBy;
    }

    public virtual void Update(Guid updatedBy)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
