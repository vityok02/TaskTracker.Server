using Application.Abstract.Interfaces;
using Domain.Entities;

namespace Application.Modules.Projects;

public static class ProjectDefaults
{
    public static IEnumerable<StateEntity> GetDefaultStates(
        Guid projectId,
        Guid userId,
        DateTime createdDate)
    {
        IEnumerable<StateEntity> states =
        [
            new StateEntity
            {
                Id = Guid.NewGuid(),
                SortOrder = 1,
                Name = "To Do",
                ProjectId = projectId,
                CreatedBy = userId,
                CreatedAt = createdDate
            },

            new StateEntity
            {
                Id = Guid.NewGuid(),
                SortOrder = 2,
                Name = "In Progress",
                ProjectId = projectId,
                CreatedBy = userId,
                CreatedAt = createdDate
            },

            new StateEntity
            {
                Id = Guid.NewGuid(),
                SortOrder = 3,
                Name = "Done",
                ProjectId = projectId,
                CreatedBy = userId,
                CreatedAt = createdDate
            }
        ];

        return states;
    }
}