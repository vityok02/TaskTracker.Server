using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class UserRepository : BaseRepository<User, Guid>, IUserRepository
{
    public UserRepository(ISqlConnectionFactory connectionFactory) : base(connectionFactory)
    { }

    public async Task<User?> GetByEmailAsync(string email)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"SELECT * FROM [User] WHERE Email = @Email";

        var user = await connection
            .QueryFirstOrDefaultAsync<User>(query, new { Email = email });

        return user;
    }
}
