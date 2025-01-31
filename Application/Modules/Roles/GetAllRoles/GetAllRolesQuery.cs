using Application.Abstract.Messaging;

namespace Application.Modules.Roles.GetAllRoles;

public sealed record GetAllRolesQuery()
    : IQuery<IEnumerable<RoleResponse>>;
