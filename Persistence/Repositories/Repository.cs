using Application.Interfaces;
using Dapper;
using Domain.Abstract;
using Microsoft.Extensions.Logging;
using Persistence.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace Persistence.Repositories;

public class Repository<TEntity, TId> : IRepository<TEntity, TId> 
    where TEntity : BaseEntity
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<Repository<TEntity, TId>> _logger;
    private readonly IEntityAttributeValuesProvider<TEntity> _entityAttributeValuesProvider;

    public Repository(
        AppDbContext dbContext,
        ILogger<Repository<TEntity, TId>> logger,
        IEntityAttributeValuesProvider<TEntity> entityMetadataService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _entityAttributeValuesProvider = entityMetadataService;
    }

    public async Task<TEntity?> GetByIdAsync(TId id)
    {
        using var connection = _dbContext.CreateConnection();

        string tableName = _entityAttributeValuesProvider.GetTableName();
        string keyColumn = _entityAttributeValuesProvider.GetKeyColumnName()!;
        string query = $"SELECT {_entityAttributeValuesProvider.GetColumnsAsProperties()} FROM {tableName} WHERE {keyColumn} = @Id";

        return await connection.QueryFirstOrDefaultAsync<TEntity>(query, new { Id = id });
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        using var connection = _dbContext.CreateConnection();

        string tableName = _entityAttributeValuesProvider.GetTableName();
        string query = $"SELECT {_entityAttributeValuesProvider.GetColumnsAsProperties()} FROM {tableName}";

        return await connection.QueryAsync<TEntity>(query);
    }

    public async Task<TId> CreateAsync(TEntity entity)
    {
        using var connection = _dbContext.CreateConnection();

        string tableName = _entityAttributeValuesProvider.GetTableName();
        string columns = _entityAttributeValuesProvider.GetColumns(excludeKey: true);
        string properties = _entityAttributeValuesProvider.GetPropertyNames(excludeKey: true);
        string query = $"INSERT INTO [{tableName}] ({columns}) VALUES ({properties}) SELECT SCOPE_IDENTITY()";

        var id = await connection.ExecuteScalarAsync(query, entity);

        return id is not null
            ? (TId)id
            : throw new RepositoryException("Failed to create entity");
    }

    public async Task DeleteAsync(TId id)
    {
        using var connection = _dbContext.CreateConnection();

        string tableName = _entityAttributeValuesProvider.GetTableName();
        string keyColumn = _entityAttributeValuesProvider.GetKeyColumnName()!;
        string query = $"DELETE FROM {tableName} WHERE {keyColumn} = @Id";
        
        await connection.ExecuteAsync(query, new { Id = id });
    }

    public Task UpdateAsync(TEntity entity)
    {
        using var connection = _dbContext.CreateConnection();

        string tableName = _entityAttributeValuesProvider.GetTableName();
        string keyColumn = _entityAttributeValuesProvider.GetKeyColumnName()!;
        string keyProperty = _entityAttributeValuesProvider.GetKeyPropertyName()!;

        StringBuilder query = new();

        query.Append($"UPDATE {tableName} SET ");

        foreach(var property in _entityAttributeValuesProvider.GetProperties(true))
        {
            var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();

            string propertyName = property.Name;
            string columnName = columnAttribute?.Name ?? propertyName;

            query.Append($"{columnName} = @{propertyName},");
        }

        query.Remove(query.Length - 1, 1);

        query.Append($" WHERE {keyColumn} = @{keyProperty}");

        return connection.ExecuteAsync(query.ToString(), entity);
    }
}
