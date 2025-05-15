using Application.Abstract.Messaging;

namespace Application.Modules.Templates.GetAllTemplates;

public sealed record GetAllTemplatesQuery()
    : IQuery<IEnumerable<TemplateDto>>;
