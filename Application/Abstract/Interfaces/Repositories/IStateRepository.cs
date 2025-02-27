using Application.Abstract.Interfaces.Base;
using Domain.Entities;
using Domain.Models;

namespace Application.Abstract.Interfaces.Repositories;

public interface IStateRepository : IRepository<State, Guid>
{
    Task<bool> ExistsByProjectIdAndNameAsync(Guid projectId, string name);
    Task<bool> ExistsForProject(Guid stateId, Guid projectId);
    Task<IEnumerable<StateModel>> GetAllByProjectIdAsync(Guid projectId);
}
