using Application.Abstract.Interfaces.Base;
using Dapper;
using Domain.Entities;
using Persistence.Abstractions;

namespace Persistence.Repositories.Base;

public abstract class BaseRepository<TEntity, TId>
    : IRepository<TEntity, TId>
    where TEntity : BaseEntity
{
    protected readonly ISqlConnectionFactory ConnectionFactory;

    protected BaseRepository(ISqlConnectionFactory connectionFactory)
    {
        ConnectionFactory = connectionFactory;
    }

    public virtual async Task<TId> CreateAsync(TEntity entity)
    {
        using var connection = ConnectionFactory.Create();

        return await connection.InsertAsync<TId, TEntity>(entity);
    }

    public virtual async Task DeleteAsync(TId id)
    {
        using var connection = ConnectionFactory.Create();

        await connection.DeleteAsync<TEntity>(id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        using var connection = ConnectionFactory.Create();

        return await connection.GetListAsync<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        using var connection = ConnectionFactory.Create();

        return await connection.GetAsync<TEntity>(id);
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        using var connection = ConnectionFactory.Create();

        await connection.UpdateAsync(entity);
    }
}
