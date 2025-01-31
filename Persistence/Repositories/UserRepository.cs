using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Persistence.Abstractions;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class UserRepository
    : BaseRepository<User, Guid>, IUserRepository
{
    public UserRepository(ISqlConnectionFactory connectionFactory)
        : base(connectionFactory)
    { }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"SELECT COUNT(1) FROM [User] WHERE Email = @Email";

        return await connection
            .ExecuteScalarAsync<bool>(query, new { Email = email });
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"SELECT * FROM [User] WHERE Email = @Email";

        var user = await connection
            .QueryFirstOrDefaultAsync<User>(query, new { Email = email });

        return user;
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"SELECT * FROM [User] WHERE UserName = @UserName";

        var user = await connection
            .QuerySingleOrDefaultAsync<User>(query, new { UserName = userName });

        return user;
    }
}
