using Application.Abstract.Interfaces.Base;
using Domain.Entities;

namespace Application.Abstract.Interfaces.Repositories;

public interface IRoleRepository : IRepository<Role, Guid>
{
    Task<Role?> GetByNameAsync(string name);
}
