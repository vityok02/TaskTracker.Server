using Application.Abstract.Interfaces.Base;
using Domain.Entities;
using Domain.Models;

namespace Application.Abstract.Interfaces.Repositories;

public interface ITaskRepository : IRepository<TaskEntity, Guid>
{
    Task AddTagAsync(Guid taskId, Guid tagId);

    Task<bool> ExistsByNameForProjectAsync(string name, Guid projectId);

    Task<IEnumerable<TaskEntity>> GetAllByStateId(Guid stateId);

    Task<IEnumerable<TaskModel>> GetAllExtendedAsync(Guid ProjectId, string? searchTerm);

    Task<TaskModel?> GetExtendedByIdAsync(Guid id);

    Task<int> GetLastOrderAsync(Guid projectId);

    Task<bool> HasTagAsync(Guid taskId, Guid tagid);

    Task RemoveTagAsync(Guid taskId, Guid tagId);

    Task UpdateRangeAsync(IEnumerable<TaskEntity> tasks);
}
