using Domain.Shared;

namespace Domain.Errors;

public static class ProjectErrors
{
    public static Error AlreadyExists
    => new("Project.AlreadyExists", $"User already has project with name such project");

    public static Error NotFound
        => new("Project.NotFound", "Project not found.");
}
