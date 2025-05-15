using Application.Abstract.Interfaces.Base;
using Domain.Entities.Templates;

namespace Application.Abstract.Interfaces.Repositories;

public interface ITemplateRepository : IRepository<TemplateEntity, Guid>
{
    Task<IEnumerable<TemplateStateEntity>> GetStatesAsync(Guid templateId);
    Task<IEnumerable<TemplateTagEntity>> GetTagsAsync(Guid templateId);
}
