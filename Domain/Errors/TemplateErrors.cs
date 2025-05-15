using Domain.Shared;

namespace Domain.Errors;

public static class TemplateErrors
{
    public static Error NotFound
        => new("Template.NotFound", "Template not found");
}
