using Application.Abstract.Interfaces.Repositories;
using Domain;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class UserRepository : GenericRepository<User, Guid>, IUserRepository
{
    public UserRepository(ISqlConnectionFactory connectionFactory) : base(connectionFactory)
    { }
}
