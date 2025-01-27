using Application.Interfaces.Base;
using Domain;

namespace Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
}
