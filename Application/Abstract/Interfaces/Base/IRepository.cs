using Domain.Abstract;

namespace Application.Abstract.Interfaces.Base;

public interface IRepository<TEntity, TId>
    where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(TId id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> GetAllAsync(object whereConditions);

    Task<IEnumerable<TEntity>> GetAllAsync(string whereConditions, object parameters);

    Task<TId> CreateAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TId id);
}
