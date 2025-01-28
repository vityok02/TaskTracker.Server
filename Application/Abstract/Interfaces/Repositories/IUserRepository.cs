using Application.Abstract.Interfaces.Base;
using Domain;

namespace Application.Abstract.Interfaces.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
}
