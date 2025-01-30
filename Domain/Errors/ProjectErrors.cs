using Domain.Abstract;

namespace Domain.Errors;

public static class ProjectErrors
{
    public static Func<string, Error> AlreadyExists => name
        => new("Project.AlreadyExists", $"Project with name {name} already exists.");
}
