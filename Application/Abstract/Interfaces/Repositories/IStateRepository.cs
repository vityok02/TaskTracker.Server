using Application.Abstract.Interfaces.Base;
using Domain.Entities;
using Domain.Models;

namespace Application.Abstract.Interfaces.Repositories;

public interface IStateRepository : IRepository<StateEntity, Guid>
{
    Task<bool> ExistsForProject(Guid stateId, Guid projectId);

    Task<IEnumerable<StateModel>> GetAllExtendedAsync(Guid projectId);

    Task<IEnumerable<StateEntity>> GetAllAsync(Guid projectId);

    Task<StateModel?> GetExtendedByIdAsync(Guid id);

    Task<int> GetLastOrderAsync(Guid ProjectId);

    Task UpdateRangeAsync(IEnumerable<StateEntity> states);
    Task CreateManyAsync(IEnumerable<StateEntity> states);
}
