using Application.Abstract.Interfaces.Base;
using Domain.Entities;
using Domain.Models;

namespace Application.Abstract.Interfaces.Repositories;

public interface ITaskRepository : IRepository<TaskEntity, Guid>
{
    Task<bool> ExistsByNameForProjectAsync(string name, Guid projectId);

    Task<IEnumerable<TaskModel>> GetAllExtendedAsync(Guid ProjectId);

    Task<TaskModel?> GetExtendedByIdAsync(Guid id);
}
