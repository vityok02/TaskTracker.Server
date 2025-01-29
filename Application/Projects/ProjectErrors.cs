using Domain.Abstract;

namespace Application.Projects;

public static class ProjectErrors
{
    public static Func<string, Error> AlreadyExists => name 
        => new("Project.AlreadyExists", $"Project with name {name} already exists.");
}
