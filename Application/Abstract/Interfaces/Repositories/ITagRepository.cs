using Application.Abstract.Interfaces.Base;
using Domain.Entities;

namespace Application.Abstract.Interfaces.Repositories;

public interface ITagRepository : IRepository<TagEntity, Guid>
{
    Task<IEnumerable<TagEntity>> GetAllAsync(Guid projectId);
}
