using Application.Abstract.Interfaces.Base;
using Domain.Entities;
using Domain.Models;

namespace Application.Abstract.Interfaces.Repositories;

public interface ITaskRepository : IRepository<AppTask, Guid>
{
    new Task<TaskModel> CreateAsync(AppTask task);

    Task<bool> ExistsByNameForProjectAsync(string name, Guid projectId);

    Task<IEnumerable<TaskModel>> GetAllByProjectIdAsync(Guid ProjectId);

    Task<TaskModel?> GetModelByIdAsync(Guid id);
}
