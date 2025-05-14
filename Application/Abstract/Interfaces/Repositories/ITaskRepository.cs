using Application.Abstract.Interfaces.Base;
using Domain.Entities;
using Domain.Models;

namespace Application.Abstract.Interfaces.Repositories;

public interface ITaskRepository : IRepository<TaskEntity, Guid>
{
    Task AddTagAsync(TaskTagEntity taskTag);

    Task<bool> ExistsByNameForProjectAsync(string name, Guid projectId);

    Task<IEnumerable<TaskEntity>> GetAllByStateId(Guid stateId);

    Task<IEnumerable<TaskModel>> GetAllExtendedAsync(Guid projectId, string? searchTerm, IEnumerable<Guid>? tagIds);

    Task<TaskModel?> GetExtendedByIdAsync(Guid id);

    Task<int> GetLastOrderAsync(Guid projectId);

    Task<IEnumerable<TaskTagEntity>> GetTagsAsync(Guid taskId);

    Task<TaskTagEntity?> GetTaskTagAsync(Guid taskId, Guid tagid);

    Task RemoveTagAsync(Guid taskId, Guid tagId);

    Task UpdateRangeAsync(IEnumerable<TaskEntity> tasks);
}
