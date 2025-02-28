using Application.Abstract.Interfaces.Base;
using Domain.Entities;

namespace Application.Abstract.Interfaces.Repositories;

public interface IProjectMemberRepository
{
    Task<ProjectMemberEntity?> GetAsync(Guid userId, Guid projectId);

    Task<RoleEntity> GetMemberRole(Guid userId, Guid projectId);

    Task CreateMember(Guid userId, Guid projectId, Guid roleId);

    Task<IEnumerable<ProjectMemberEntity>> GetAllAsync(Guid userId, Guid projectId);
}
