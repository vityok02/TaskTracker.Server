using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Persistence.Abstractions;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class RoleRepository
    : BaseRepository<Role, Guid>, IRoleRepository
{
    public RoleRepository(ISqlConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"SELECT * FROM [Role]
            WHERE [Name] = @Name";

        return await connection
            .QueryFirstAsync<Role>(query, new { Name = name });
    }
}
