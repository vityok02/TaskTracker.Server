using Application.Abstract.Interfaces.Base;
using Domain.Abstract;
using Domain.Entities;
using Domain.Models;

namespace Application.Abstract.Interfaces.Repositories;

public interface IProjectRepository : IRepository<ProjectEntity, Guid>
{
    Task<bool> ExistsByNameAsync(Guid userId, string projectName);

    Task<Guid> CreateAsync(ProjectEntity project, Guid roleId);

    Task<ProjectModel?> GetExtendedByIdAsync(Guid projectId, Guid userId);

    Task<PagedList<ProjectModel>> GetPagedAsync(
        int currentPageNumber,
        int pageSize,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        Guid userId);
}
