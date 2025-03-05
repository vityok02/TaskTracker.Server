using Domain.Entities;
using Domain.Models;

namespace Application.Abstract.Interfaces.Repositories;

public interface IProjectMemberRepository
{
    Task<ProjectMemberModel> CreateAsync(Guid userId, Guid projectId, Guid roleId);

    Task<ProjectMemberModel?> GetAsync(Guid userId, Guid projectId);

    Task<IEnumerable<ProjectMemberModel>> GetAllAsync(Guid projectId);
}
