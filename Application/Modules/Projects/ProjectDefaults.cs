using Domain.Entities;

namespace Application.Modules.Projects;

public static class ProjectDefaults
{
    public static IEnumerable<State> GetDefaultStates(Guid projectId)
    {
        IEnumerable<State> states =
        [
            new State { Id = Guid.NewGuid(), Number = 1, Name = "To Do", ProjectId = projectId },
            new State { Id = Guid.NewGuid(), Number = 2, Name = "In Progress", ProjectId = projectId },
            new State { Id = Guid.NewGuid(), Number = 3, Name = "Done", ProjectId = projectId }
        ];

        return states;
    }
}