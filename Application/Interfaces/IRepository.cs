using Domain.Abstract;

namespace Application.Interfaces;

public interface IRepository<TEntity> 
    where TEntity : BaseEntity
{
    Task<TEntity> GetByIdAsync(Guid id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task CreateAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(Guid id);
}
