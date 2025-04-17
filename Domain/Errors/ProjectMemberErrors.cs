using Domain.Shared;

namespace Domain.Errors;

public static class ProjectMemberErrors
{
    public static Error NotFound
        => new("ProjectMember.NotFound", "Project member not found.");

    public static Error AlreadyExists
        => new("ProjectMember.AlreadyExists", "Project member already exists.");

    public static Error CannotDeleteYourself
        => new("ProjectMember.InvalidOperation", "You cannot delete yourself from the project.");

    public static Error CannotUpdateYourself
        => new("ProjectMember.InvalidOperation", "You cannot change your role.");
}
