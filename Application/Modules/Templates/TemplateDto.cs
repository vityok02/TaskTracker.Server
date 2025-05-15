namespace Application.Modules.Templates;

public record TemplateDto(
    Guid Id,
    string Name,
    string? Description,
    int SortOrder);
