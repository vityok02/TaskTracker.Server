using Domain.Entities;
using Domain.Models;

namespace Application.Abstract.Interfaces.Repositories;

public interface IProjectMemberRepository
{
    Task<ProjectMemberModel> CreateAsync(Guid userId, Guid projectId, Guid roleId);

    Task<ProjectMemberEntity?> GetAsync(Guid userId, Guid projectId);

    Task<ProjectMemberModel?> GetExtendedAsync(Guid userId, Guid projectId);

    Task<IEnumerable<ProjectMemberModel>> GetAllExtendedAsync(Guid projectId);

    Task UpdateAsync(ProjectMemberEntity projectMember);

    Task DeleteAsync(ProjectMemberEntity projectMember);
}
