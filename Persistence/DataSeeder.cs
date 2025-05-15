using Dapper;
using Domain.Constants;
using Domain.Entities.Templates;
using Persistence.Abstractions;
using Z.Dapper.Plus;

namespace Persistence;

public static class DataSeeder
{
    public static async Task SeedDataAsync(ISqlConnectionFactory connectionFactory)
    {
        using var connection = connectionFactory.Create();

        var templateCount = await connection
            .ExecuteScalarAsync<int>("SELECT COUNT(1) FROM Template");

        if (templateCount > 0)
            return;

        await SeedBasicKanbanAsync(connection);
        await SeedSoftwareDevelopmentAsync(connection);
    }

    private static async Task SeedBasicKanbanAsync(System.Data.IDbConnection connection)
    {
        var basicKanbanId = Guid.NewGuid();

        var template = new TemplateEntity
        {
            Id = basicKanbanId,
            Name = "Basic Kanban",
            Description = "Basic Kanban template",
            SortOrder = 1
        };

        await connection
            .InsertAsync<Guid, TemplateEntity>(template);

        var basicKanbanStates = new List<TemplateStateEntity>()
        {
            new() { Id = Guid.NewGuid(), Name = "To Do", Color = Colors.Blue, SortOrder = 1, TemplateId = basicKanbanId },
            new() { Id = Guid.NewGuid(), Name = "In Progress", Color = Colors.Yellow, SortOrder = 2, TemplateId = basicKanbanId },
            new() { Id = Guid.NewGuid(), Name = "Done", Color = Colors.Green, SortOrder = 3, TemplateId = basicKanbanId },
        };

        await connection
            .BulkInsertAsync(basicKanbanStates);

        var basicKanbanTags = new List<TemplateTagEntity>()
        {
            new() { Id = Guid.NewGuid(), Name = "Urgent", Color = Colors.Red, SortOrder = 1, TemplateId = basicKanbanId },
            new() { Id = Guid.NewGuid(), Name = "Medium", Color = Colors.Orange, SortOrder = 2, TemplateId = basicKanbanId },
            new() { Id = Guid.NewGuid(), Name = "Low", Color = Colors.Green, SortOrder = 3, TemplateId = basicKanbanId },
            new() { Id = Guid.NewGuid(), Name = "Idea", Color = Colors.Pink, SortOrder = 4, TemplateId = basicKanbanId },
            new() { Id = Guid.NewGuid(), Name = "Cleanup", Color = Colors.Purple, SortOrder = 5, TemplateId = basicKanbanId },
        };

        await connection
            .BulkInsertAsync(basicKanbanTags);
    }

    private static async Task SeedSoftwareDevelopmentAsync(System.Data.IDbConnection connection)
    {
        var softwareDevId = Guid.NewGuid();

        var template = new TemplateEntity
        {
            Id = softwareDevId,
            Name = "Software Development",
            Description = "Template for software projects",
            SortOrder = 2
        };

        await connection
            .InsertAsync<Guid, TemplateEntity>(template);

        var softwareDevStates = new List<TemplateStateEntity>()
        {
            new() { Id = Guid.NewGuid(), Name = "Backlog", Color = Colors.Pink, SortOrder = 1, TemplateId = softwareDevId },
            new() { Id = Guid.NewGuid(), Name = "To Do", Color = Colors.Yellow, SortOrder = 2, TemplateId = softwareDevId },
            new() { Id = Guid.NewGuid(), Name = "In Progress", Color = Colors.Blue, SortOrder = 3, TemplateId = softwareDevId },
            new() { Id = Guid.NewGuid(), Name = "In Review", Color = Colors.Orange, SortOrder = 4, TemplateId = softwareDevId },
            new() { Id = Guid.NewGuid(), Name = "Testing", Color = Colors.Cyan, SortOrder = 5, TemplateId = softwareDevId },
            new() { Id = Guid.NewGuid(), Name = "Done", Color = Colors.Green, SortOrder = 6, TemplateId = softwareDevId },
        };

        await connection
            .BulkInsertAsync(softwareDevStates);

        var softwareDevTags = new List<TemplateTagEntity>()
        {
            new() { Id = Guid.NewGuid(), Name = "Bug", Color = Colors.Red, SortOrder = 1, TemplateId = softwareDevId },
            new() { Id = Guid.NewGuid(), Name = "Feature", Color = Colors.Blue, SortOrder = 2, TemplateId = softwareDevId },
            new() { Id = Guid.NewGuid(), Name = "Refactor", Color = Colors.Gray, SortOrder = 3, TemplateId = softwareDevId },
            new() { Id = Guid.NewGuid(), Name = "Release", Color = Colors.Cyan, SortOrder = 4, TemplateId = softwareDevId },
            new() { Id = Guid.NewGuid(), Name = "Test", Color = Colors.Green, SortOrder = 5, TemplateId = softwareDevId },
            new() { Id = Guid.NewGuid(), Name = "Architecture", Color = Colors.Orange, SortOrder = 6, TemplateId = softwareDevId },
            new() { Id = Guid.NewGuid(), Name = "WIP", Color = Colors.Yellow, SortOrder = 7, TemplateId = softwareDevId },
        };

        await connection
            .BulkInsertAsync(softwareDevTags);
    }
}
