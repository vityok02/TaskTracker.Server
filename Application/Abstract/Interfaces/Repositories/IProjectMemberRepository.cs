using Application.Abstract.Interfaces.Base;
using Domain;

namespace Application.Abstract.Interfaces.Repositories;

public interface IProjectRepository<TId> : IRepository<Project, TId>
{
    Task<TId> CreateAsync(Guid userId, Project project);
}
