using Application.Abstract.Interfaces.Base;
using Domain.Entities;

namespace Application.Abstract.Interfaces.Repositories;

public interface IRoleRepository : IRepository<RoleEntity, Guid>
{
    Task<RoleEntity?> GetByNameAsync(string name);

    Task<RoleEntity> GetMemberRole(Guid userId, Guid projectId);
}
