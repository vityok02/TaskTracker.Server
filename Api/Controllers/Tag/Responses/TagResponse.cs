using Api.Controllers.Abstract;

namespace Api.Controllers.Tag.Responses;

public class TagResponse : AuditableResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Color { get; set; } = null!;

    public int SortOrder { get; set; }

    public Guid ProjectId { get; set; }
}
