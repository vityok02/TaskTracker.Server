using Domain.Entities;

namespace Application.Modules.Projects;

public static class ProjectDefaults
{
    public static IEnumerable<StateEntity> GetDefaultStates(Guid projectId)
    {
        IEnumerable<StateEntity> states =
        [
            new StateEntity { Id = Guid.NewGuid(), Number = 1, Name = "To Do", ProjectId = projectId },
            new StateEntity { Id = Guid.NewGuid(), Number = 2, Name = "In Progress", ProjectId = projectId },
            new StateEntity { Id = Guid.NewGuid(), Number = 3, Name = "Done", ProjectId = projectId }
        ];

        return states;
    }
}