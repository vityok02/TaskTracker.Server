using Domain.Abstract;

namespace Application.Interfaces.Base;

public interface IRepository<TEntity, TId>
    where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(TId id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TId> CreateAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TId id);
}
