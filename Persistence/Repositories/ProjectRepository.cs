using Application.Abstract.Interfaces.Repositories;
using Application.Modules.Projects;
using Dapper;
using Domain.Entities;
using Domain.Models;
using Persistence.Abstractions;
using Persistence.Repositories.Base;
using System.Linq;

namespace Persistence.Repositories;

public class ProjectRepository
    : BaseRepository<ProjectEntity, Guid>, IProjectRepository
{
    public ProjectRepository(ISqlConnectionFactory connectionFactory)
        : base(connectionFactory)
    { }

    public async Task<bool> ExistsByNameAsync(string projectName)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT COUNT(1) 
            FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
            JOIN [User] u ON u.Id = pm.UserId
            WHERE p.Name = @Name";

        var result = await connection
            .ExecuteScalarAsync<bool>(
                query,
                new
                {
                    Name = projectName
                });

        return result;
    }

    public async Task<Guid> CreateAsync(ProjectEntity project, Guid roleId)
    {
        // Posible refactor: combine queries

        using var connection = ConnectionFactory
            .Create();

        var projectId = await connection
            .InsertAsync<Guid, ProjectEntity>(project);

        var states = ProjectDefaults.GetDefaultStates(projectId);

        string query = (@"INSERT INTO ProjectMember(UserId, ProjectId, RoleId)
            VALUES(@UserId, @ProjectId, @RoleId)");

        var insertStatesSql = @"INSERT INTO State(Id, Number, Name, ProjectId)
            VALUES (@Id, @Number, @Name, @ProjectId);";

        await connection
            .ExecuteAsync(insertStatesSql, states);

        await connection
            .ExecuteAsync(
                query,
                new
                {
                    UserId = project.CreatedBy,
                    ProjectId = projectId,
                    RoleId = roleId
                });

        return projectId;
    }

    public async Task<IEnumerable<ProjectModel>> GetAllByUserIdAsync(Guid userId)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT 
                p.Id AS Id,
                p.Name,
                p.Description,
                p.CreatedAt,
                p.UpdatedAt,
                uc.Id AS CreatedBy,
                uc.Username AS CreatedByName,
                uu.Id AS UpdatedBy,
                uu.Username AS UpdatedByName,
                s.Id AS Id,
                s.Name AS Name,
                s.Number AS Number
            FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
            JOIN [User] uc ON p.CreatedBy = uc.Id
            LEFT JOIN [User] uu ON p.UpdatedBy = uu.Id
            JOIN [State] s ON s.ProjectId = p.Id
            WHERE pm.UserId = @UserId";

        var lookup = new Dictionary<Guid, ProjectModel>();

        await connection.QueryAsync<ProjectModel, ProjectStateModel, ProjectModel>(
            query,
            (project, state) => MapProjectStates(project, state, lookup),
            new { UserId = userId },
            splitOn: "Id"
        );

        return lookup.Values;
    }

    public async Task<ProjectModel?> GetExtendedByIdAsync(Guid projectId)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT 
                p.Id,
                p.Name,
                p.Description,
                p.CreatedAt,
                p.UpdatedAt,
                uc.Id AS CreatedBy,
                uc.Username AS CreatedByName,
                uu.Id AS UpdatedBy,
                uu.Username AS UpdatedByName,
                s.Id AS Id,
                s.Name AS Name,
                s.Number AS Number
            FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
            JOIN [User] uc ON p.CreatedBy = uc.Id
            LEFT JOIN [User] uu ON p.UpdatedBy = uu.Id
            JOIN [State] s ON s.ProjectId = p.Id
            WHERE pm.ProjectId = @ProjectId";

        var lookup = new Dictionary<Guid, ProjectModel>();

        await connection.QueryAsync<ProjectModel, ProjectStateModel, ProjectModel>(
            query,
            (project, state) => MapProjectStates(project, state, lookup),
            new { ProjectId = projectId },
            splitOn: "Id"
        );

        return lookup.Values
            .FirstOrDefault();
    }

    public override async Task UpdateAsync(ProjectEntity project)
    {
        using var connection = ConnectionFactory.Create();

        await connection.UpdateAsync(project);
    }

    public override async Task DeleteAsync(Guid id)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            DELETE FROM [Project] WHERE Id = @ProjectId";

        await connection.ExecuteAsync(
            query,
            new { ProjectId = id });
    }

    private static ProjectModel MapProjectStates(
    ProjectModel project,
    ProjectStateModel state,
    Dictionary<Guid, ProjectModel> lookup)
    {
        if (!lookup.TryGetValue(project.Id, out var existingProject))
        {
            existingProject = project;
            existingProject.States = [];
            lookup.Add(existingProject.Id, existingProject);
        }

        existingProject.States.Add(state);
        return existingProject;
    }
}
