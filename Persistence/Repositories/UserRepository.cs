using Application.Interfaces;
using Dapper;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Task = System.Threading.Tasks.Task;

namespace Persistence.Repositories;

public class UserRepository : IRepository<User>
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("localdb")!;
    }

    public async Task CreateAsync(User user)
    {
        using IDbConnection connection = new SqlConnection(_connectionString);

        var query = "INSERT INTO [User] (Id, UserName) VALUES (@Id, @UserName)";

        var userParameters = new { user.Id, user.UserName };

        await connection.ExecuteAsync(query, userParameters);
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }
}
