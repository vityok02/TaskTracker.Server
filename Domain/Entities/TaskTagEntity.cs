namespace Domain.Entities;

public class TaskTagEntity
{
    public Guid TaskId { get; set; }

    public Guid TagId { get; set; }

    public int SortOrder { get; set; }
}
