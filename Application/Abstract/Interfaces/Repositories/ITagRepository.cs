using Application.Abstract.Interfaces.Base;
using Domain.Entities;

namespace Application.Abstract.Interfaces.Repositories;

public interface ITagRepository : IRepository<TagEntity, Guid>
{
    Task CreateManyAsync(IEnumerable<TagEntity> tags);
    Task<IEnumerable<TagEntity>> GetAllAsync(Guid projectId);
    Task UpdateRangeAsync(IEnumerable<TagEntity> tags);
}
