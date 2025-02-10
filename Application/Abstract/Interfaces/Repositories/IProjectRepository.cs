using Application.Abstract.Interfaces.Base;
using Domain.Entities;

namespace Application.Abstract.Interfaces.Repositories;

public interface IProjectRepository : IRepository<Project, Guid>
{
    Task<bool> ExistsByNameAsync(Guid userId, string projectName);

    Task<Guid> CreateAsync(Project project, Guid roleId);

    Task<IEnumerable<Project>> GetAllAsync(Guid userId);
}
