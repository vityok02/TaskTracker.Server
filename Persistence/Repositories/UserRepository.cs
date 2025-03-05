using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Persistence.Abstractions;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class UserRepository
    : BaseRepository<UserEntity, Guid>, IUserRepository
{
    public UserRepository(ISqlConnectionFactory connectionFactory)
        : base(connectionFactory)
    { }

    public async Task<UserEntity?> GetUserByEmailOrNameAsync(string email, string username)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"SELECT * FROM [User] WHERE Email = @Email OR Username = @Username";

        return await connection
            .QuerySingleOrDefaultAsync<UserEntity>(
            query, new
            {
                Email = email,
                Username = username
            });
    }

    public async Task<UserEntity?> GetByEmailAsync(string email)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"SELECT * FROM [User] WHERE Email = @Email";

        var user = await connection
            .QuerySingleOrDefaultAsync<UserEntity>(query, new { Email = email });

        return user;
    }

    public async Task<IEnumerable<UserEntity>> GetByNameContainsAsync(string userName)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"SELECT TOP (8) * FROM [User] WHERE Username LIKE @Username";

        return await connection
            .QueryAsync<UserEntity>(query, new { Username = $"%{userName}%" });
    }
}
