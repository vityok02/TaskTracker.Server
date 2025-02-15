using Application.Abstract.Interfaces.Base;
using Application.Modules.Projects;
using Domain.Entities;

namespace Application.Abstract.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<bool> ExistsByNameAsync(Guid userId, string projectName);

    Task<Guid> CreateAsync(Project project, Guid roleId);

    Task<IEnumerable<ProjectDto>> GetAllAsync(Guid userId);

    Task<ProjectDto?> GetByIdAsync(Guid userId, Guid projectId);
}
