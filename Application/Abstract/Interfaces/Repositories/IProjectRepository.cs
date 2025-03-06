using Application.Abstract.Interfaces.Base;
using Domain.Entities;
using Domain.Models;

namespace Application.Abstract.Interfaces.Repositories;

public interface IProjectRepository : IRepository<ProjectEntity, Guid>
{
    Task<bool> ExistsByNameAsync(Guid userId, string projectName);

    Task<Guid> CreateAsync(ProjectEntity project, Guid roleId);

    Task<IEnumerable<ProjectModel>> GetAllByUserIdAsync(Guid userId);

    Task<ProjectModel?> GetExtendedByIdAsync(Guid projectId);
}
