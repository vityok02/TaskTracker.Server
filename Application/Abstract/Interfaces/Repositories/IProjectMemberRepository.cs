using Application.Abstract.Interfaces.Base;
using Domain.Entities;

namespace Application.Abstract.Interfaces.Repositories;

public interface IProjectMemberRepository
{
    Task<ProjectMember?> GetAsync(Guid userId, Guid projectId);

    Task<Role> GetMemberRole(Guid userId, Guid projectId);

    Task CreateMember(Guid userId, Guid projectId, Guid roleId);

    Task<IEnumerable<ProjectMember>> GetAllAsync(Guid userId, Guid projectId);
}
