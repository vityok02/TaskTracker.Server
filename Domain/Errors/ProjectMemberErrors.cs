using Domain.Abstract;

namespace Domain.Errors;

public static class ProjectMemberErrors
{
    public static Error NotFound
        => new("ProjectMember.NotFound", "Project member not found.");

    public static Error AlreadyExists
        => new("ProjectMember.AlreadyExists", "Project member already exists.");
}
