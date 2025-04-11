using Application.Abstract.Interfaces.Repositories;
using Application.Modules.Projects;
using Dapper;
using Domain.Abstract;
using Domain.Entities;
using Domain.Models;
using Persistence.Abstractions;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class ProjectRepository
    : BaseRepository<ProjectEntity, Guid>, IProjectRepository
{
    public ProjectRepository(ISqlConnectionFactory connectionFactory)
        : base(connectionFactory)
    { }

    public async Task<PagedList<ProjectModel>> GetPagedAsync(
        int currentPageNumber,
        int pageSize,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        Guid userId)
    {
        using var connection = ConnectionFactory.Create();

        int skip = (currentPageNumber - 1) * pageSize;
        int take = pageSize;

        sortColumn = sortColumn switch
        {
            "Name" => "p.Name",
            "CreatedAt" => "p.CreatedAt",
            "UpdatedAt" => "p.UpdatedAt",
            _ => "p.CreatedAt"
        };

        sortOrder = sortOrder?.ToLower() == "desc"
            ? "DESC"
            : "ASC";

        var query = @$"
            SELECT
            COUNT(*)
            FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId 
            WHERE pm.UserId = @UserId 
                AND p.Name LIKE @SearchTerm

            SELECT
                p.Id AS Id,
                p.Name,
                p.Description,
                p.CreatedAt,
                p.UpdatedAt,
                uc.Id AS CreatedBy,
                uc.Username AS CreatedByName,
                uu.Id AS UpdatedBy,
                uu.Username AS UpdatedByName
            FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
            JOIN [User] uc ON p.CreatedBy = uc.Id
            LEFT JOIN [User] uu ON p.UpdatedBy = uu.Id
            WHERE pm.UserId = @UserId 
                AND p.Name LIKE @SearchTerm
            ORDER BY {sortColumn} {sortOrder}
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

        var reader = await connection
            .QueryMultipleAsync(
                query,
                new
                {
                    Skip = skip,
                    Take = take,
                    UserId = userId,
                    SearchTerm = $"%{searchTerm}%",
                    SortColumn = sortColumn,
                    SortOrder = sortOrder
                });

        var totalCount = await reader
            .ReadFirstOrDefaultAsync<int>();

        var projects = await reader
            .ReadAsync<ProjectModel>();

        return new PagedList<ProjectModel>(
            totalCount, projects, currentPageNumber, pageSize);
    }

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
            .ExecuteScalarAsync<bool>(
                query,
                new
                {
                    Name = projectName,
                    UserId = userId
                });

        return result;
    }

    public async Task<Guid> CreateAsync(ProjectEntity project, Guid roleId)
    {
        // TODO: add transaction

        using var connection = ConnectionFactory
            .Create();

        var projectId = await connection
            .InsertAsync<Guid, ProjectEntity>(project);

        var states = ProjectDefaults.GetDefaultStates(projectId, project.CreatedBy, project.CreatedAt);

        string query = @"INSERT INTO ProjectMember(UserId, ProjectId, RoleId)
            VALUES(@UserId, @ProjectId, @RoleId)";

        var insertStatesSql = @"INSERT INTO State(Id, SortOrder, Name, CreatedBy, CreatedAt, ProjectId)
            VALUES (@Id, @SortOrder, @Name, @CreatedBy, @CreatedAt, @ProjectId);";

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
                s.Description AS Description,
                s.Color AS Color,
                s.SortOrder AS SortOrder
            FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
            JOIN [User] uc ON p.CreatedBy = uc.Id
            LEFT JOIN [User] uu ON p.UpdatedBy = uu.Id
            JOIN [State] s ON s.ProjectId = p.Id
            WHERE pm.UserId = @UserId
            ORDER BY p.CreatedAt";

        var lookup = new Dictionary<Guid, ProjectModel>();

        await connection.QueryAsync<ProjectModel, StateModel, ProjectModel>(
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
            SELECT DISTINCT
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
                s.Description AS Description,
                s.Color AS Color,
                s.SortOrder AS SortOrder
            FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
            JOIN [User] uc ON p.CreatedBy = uc.Id
            LEFT JOIN [User] uu ON p.UpdatedBy = uu.Id
            LEFT JOIN [State] s ON s.ProjectId = p.Id
            WHERE pm.ProjectId = @ProjectId";

        var lookup = new Dictionary<Guid, ProjectModel>();

        await connection.QueryAsync<ProjectModel, StateModel, ProjectModel>(
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
    StateModel state,
    Dictionary<Guid, ProjectModel> lookup)
    {
        if (!lookup.TryGetValue(project.Id, out ProjectModel? value))
        {
            value = project;
            lookup[project.Id] = value;
        }

        value.States ??= new List<StateModel>();
        value.States.Add(state);

        return project;
    }
}
