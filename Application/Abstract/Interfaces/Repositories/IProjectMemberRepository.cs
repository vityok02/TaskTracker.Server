using Application.Abstract.Interfaces.Base;
using Domain.Entities;

namespace Application.Abstract.Interfaces.Repositories;

public interface IProjectRepository : IRepository<Project, Guid>
{
    Task<bool> ExistsAsync(Guid userId, string projectName);

    new Task<Guid> CreateAsync(Project project);
}
