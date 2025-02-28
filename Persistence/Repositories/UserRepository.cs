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
            .QueryFirstOrDefaultAsync<UserEntity>(
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
            .QueryFirstOrDefaultAsync<UserEntity>(query, new { Email = email });

        return user;
    }

    public async Task<UserEntity?> GetByNameAsync(string userName)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"SELECT * FROM [User] WHERE UserName = @UserName";

        var user = await connection
            .QuerySingleOrDefaultAsync<UserEntity>(query, new { UserName = userName });

        return user;
    }
}
