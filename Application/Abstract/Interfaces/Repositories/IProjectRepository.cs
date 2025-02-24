using Application.Abstract.Interfaces.Base;
using Domain.Entities;
using Domain.Models;

namespace Application.Abstract.Interfaces.Repositories;

public interface IProjectRepository : IRepository<Project, Guid>
{
    Task<bool> ExistsByNameAsync(Guid userId, string projectName);

    Task<Guid> CreateAsync(Project project, Guid roleId);

    Task<IEnumerable<ProjectModel>> GetAllAsync(Guid userId);

    Task<ProjectModel?> GetModelByUserIdAndProjectIdAsync(Guid userId, Guid projectId);
}
