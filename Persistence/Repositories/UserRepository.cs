using Application.Abstract.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class UserRepository : BaseRepository<User, Guid>, IUserRepository
{
    public UserRepository(ISqlConnectionFactory connectionFactory) : base(connectionFactory)
    { }
}
