using Application.Abstract.Interfaces.Base;
using Domain.Entities;
using Domain.Models;

namespace Application.Abstract.Interfaces.Repositories;

public interface IStateRepository : IRepository<StateEntity, Guid>
{
    Task<bool> ExistsForProject(Guid stateId, Guid projectId);

    Task<IEnumerable<StateModel>> GetAllByProjectIdAsync(Guid projectId);
    Task<(StateEntity state1, StateEntity state2)> GetBothByIdsAsync(Guid stateId1, Guid stateId2);
    Task<StateModel?> GetExtendedByIdAsync(Guid id);

    Task<int> GetLastStateNumberAsync(Guid ProjectId);
    Task UpdateBothAsync(StateEntity state1, StateEntity state2);
}
