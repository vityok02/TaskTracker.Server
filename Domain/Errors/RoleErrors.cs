using Domain.Shared;

namespace Domain.Errors;

public static class RoleErrors
{
    public static Error NotFound
        => new("Role.NotFound", "Role not found.");
}
