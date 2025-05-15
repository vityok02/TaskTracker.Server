namespace Api.Controllers.Template;

public record TemplateResponse(
    Guid Id,
    string Name,
    string? Description,
    int SortOrder);
