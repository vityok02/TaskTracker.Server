using Domain.Entities;

namespace Domain.Abstract;

public class SortableEntity : AuditableEntity
{
    public int SortOrder { get; set; }
}
