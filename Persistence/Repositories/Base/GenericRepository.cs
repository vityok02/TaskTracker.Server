using Application.Abstract.Interfaces.Base;
using Dapper;
using Domain.Abstract;

namespace Persistence.Repositories.Base;

public class GenericRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : BaseEntity
{
    protected readonly ISqlConnectionFactory _connectionFactory;

    public GenericRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<TId> CreateAsync(TEntity entity)
    {
        using var connection = _connectionFactory.Create();

        return await connection.InsertAsync<TId, TEntity>(entity);
    }

    public async Task DeleteAsync(TId id)
    {
        using var connection = _connectionFactory.Create();

        await connection.DeleteAsync<TEntity>(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        using var connection = _connectionFactory.Create();

        return await connection.GetListAsync<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(TId id)
    {
        using var connection = _connectionFactory.Create();

        return await connection.GetAsync<TEntity>(id);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        using var connection = _connectionFactory.Create();

        await connection.UpdateAsync(entity);
    }
}
