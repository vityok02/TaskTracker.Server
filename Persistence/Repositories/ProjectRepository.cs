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

        var totalCountQuery = @$"
            SELECT
            COUNT(*)
            FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId 
            WHERE pm.UserId = @UserId AND p.Name LIKE @SearchTerm";

        var totalCount = await connection
            .ExecuteScalarAsync<int>(
                totalCountQuery,
                new
                {
                    UserId = userId,
                    SearchTerm = $"%{searchTerm}%"
                });

        var projectsQuery = @$"
            {GetProjectQuery()}
            WHERE pm.UserId = @UserId 
                AND p.Name LIKE @SearchTerm
            ORDER BY {sortColumn} {sortOrder}
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

        var projects = await connection
             .QueryAsync<ProjectModel, RoleEntity, ProjectModel>(
                 sql: projectsQuery,
                 map: (project, role) =>
                 {
                     project.Role = role;
                     return project;
                 },
                 param: new
                 {
                     Skip = skip,
                     Take = take,
                     UserId = userId,
                     SearchTerm = $"%{searchTerm}%",
                     SortColumn = sortColumn,
                     SortOrder = sortOrder
                 },
                 splitOn: "Id");

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
        using var connection = ConnectionFactory.Create();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var projectId = await connection.InsertAsync<Guid, ProjectEntity>(project, transaction);

            var states = ProjectDefaults.GetDefaultStates(projectId, project.CreatedBy, project.CreatedAt);

            string insertMemberQuery = @"
                INSERT INTO ProjectMember(UserId, ProjectId, RoleId)
                VALUES(@UserId, @ProjectId, @RoleId)";

            //string insertStatesQuery = @"
            //    INSERT INTO State(Id, SortOrder, Name, CreatedBy, CreatedAt, ProjectId)
            //    VALUES (@Id, @SortOrder, @Name, @CreatedBy, @CreatedAt, @ProjectId)";

            //await connection.ExecuteAsync(insertStatesQuery, states, transaction);

            await connection.ExecuteAsync(insertMemberQuery, new
            {
                UserId = project.CreatedBy,
                ProjectId = projectId,
                RoleId = roleId
            }, transaction);

            transaction.Commit();
            return projectId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<ProjectModel?> GetExtendedByIdAsync(Guid projectId, Guid userId)
    {
        using var connection = ConnectionFactory.Create();

        var query = @"
            SELECT
                p.Id AS Id,
                p.Name,
                p.Description,
                p.CreatedAt,
                p.UpdatedAt,
                p.StartDate,
                p.EndDate,
                uc.Id AS CreatedBy,
                uc.Username AS CreatedByName,
                uu.Id AS UpdatedBy,
                uu.Username AS UpdatedByName,
                s.Id,
                s.Name,
                s.Description,
                s.Color,
                s.SortOrder,
                r.Id,
                r.Name,
                r.Description
            FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
            JOIN [User] uc ON p.CreatedBy = uc.Id
            LEFT JOIN [User] uu ON p.UpdatedBy = uu.Id
            JOIN [Role] r ON r.Id = pm.RoleId
            LEFT JOIN [State] s ON s.ProjectId = p.Id
            WHERE pm.ProjectId = @ProjectId AND pm.UserId = @UserId";

        var lookup = new Dictionary<Guid, ProjectModel>();

        var projects = await connection.QueryAsync<ProjectModel, StateModel, RoleEntity, ProjectModel>(
            sql: query,
            map: (project, state, role) => MapProject(project, state, role, lookup),
            param: new
            {
                ProjectId = projectId,
                UserId = userId
            },
            splitOn: "Id, Id"
        );

        return lookup.Values
            .SingleOrDefault();
    }


    private static string GetProjectQuery()
    {
        return @"
            SELECT
                p.Id AS Id,
                p.Name,
                p.Description,
                p.CreatedAt,
                p.UpdatedAt,
                p.StartDate,
                p.EndDate,
                uc.Id AS CreatedBy,
                uc.Username AS CreatedByName,
                uu.Id AS UpdatedBy,
                uu.Username AS UpdatedByName,
                r.Id,
                r.Name,
                r.Description
            FROM [Project] p
            JOIN [ProjectMember] pm ON p.Id = pm.ProjectId
            JOIN [User] uc ON p.CreatedBy = uc.Id
            LEFT JOIN [User] uu ON p.UpdatedBy = uu.Id
            JOIN [Role] r ON r.Id = pm.RoleId";
    }

    private static ProjectModel MapProject(
        ProjectModel project,
        StateModel state,
        RoleEntity role,
        Dictionary<Guid, ProjectModel> lookup)
    {
        if (!lookup.TryGetValue(project.Id, out ProjectModel? value))
        {
            value = project;
            lookup[project.Id] = value;
        }

        value.Role = role;

        value.States ??= [];
        value.States.Add(state);

        return project;
    }
}
