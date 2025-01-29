using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class UserRepository : GenericRepository<User, Guid>, IUserRepository
{
    public UserRepository(ISqlConnectionFactory connectionFactory) : base(connectionFactory)
    { }

    public async Task<User?> GetByIdWithProjectsAsync(Guid id)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT u.*, p.*
            FROM [User] u
            LEFT JOIN ProjectMember pm ON u.id = pm.UserId
            LEFT JOIN Project p ON pm.ProjectId = p.id
            WHERE u.Id = @id
            ";

        var user = await connection.QueryFirstOrDefaultAsync<User>(query, new { id });
        return user;
    }
}
