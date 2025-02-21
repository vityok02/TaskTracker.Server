using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Domain.Models;
using Persistence.Abstractions;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

// TODO: ProjectDto, Handle such situations as update

public class ProjectRepository
    : BaseRepository<Project, Guid>, IProjectRepository
{
    public ProjectRepository(ISqlConnectionFactory connectionFactory)
        : base(connectionFactory)
    { }

    public async Task<bool> ExistsByNameAsync(Guid userId, string projectName)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT COUNT(1) 
            FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
            JOIN [User] u ON u.Id = pm.UserId
            WHERE p.Name = @Name AND pm.UserId = @UserId";

        var result = await connection
            .ExecuteScalarAsync<bool>(query,
                new
                { 
                    Name = projectName,
                    UserId = userId 
                });

        return result;
    }

    public async Task<Guid> CreateAsync(Project project, Guid roleId)
    {
        using var connection = ConnectionFactory
            .Create();

        var projectId = await connection
            .InsertAsync<Guid, Project>(project);

        var query = @"INSERT INTO ProjectMember(UserId, ProjectId, RoleId) 
            VALUES (@UserId, @ProjectId, @RoleId)";

        await connection.ExecuteAsync(query,
            new 
            { 
                UserId = project.CreatedBy,
                ProjectId = projectId,
                RoleId = roleId }
            );

        return projectId;
    }

    public async Task<IEnumerable<ProjectModel>> GetAllAsync(Guid userId)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT 
                p.Id,
                p.Name,
                p.Description,
                p.CreatedAt,
                p.UpdatedAt,
                uc.Username AS CreatedBy,
                uu.Username AS UpdatedBy
            FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
            JOIN [User] uc ON p.CreatedBy = uc.Id
            LEFT JOIN [User] uu ON p.UpdatedBy = uu.Id
            WHERE pm.UserId = @UserId";

        return await connection
            .QueryAsync<ProjectModel>(query,
                new { UserId = userId });
    }

    public async Task<ProjectModel?> GetByIdAsync(Guid userId, Guid projectId)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT 
                p.Id,
                p.Name,
                p.Description,
                p.CreatedAt,
                p.UpdatedAt,
                uc.Username AS CreatedBy,
                uu.Username AS UpdatedBy
            FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
            JOIN [User] uc ON p.CreatedBy = uc.Id
            LEFT JOIN [User] uu ON p.UpdatedBy = uu.Id
            WHERE pm.UserId = @UserId AND pm.ProjectId = p.Id";

        return await connection
            .QueryFirstOrDefaultAsync<ProjectModel>(query, 
                new 
                {
                    UserId = userId,
                    ProjectId = projectId
                });
    }
}
