using Application.Abstract.Messaging;

namespace Application.Modules.Users.SearchUserQuery;

public sealed record SearchUsersQuery(string Username)
    : IQuery<IEnumerable<UserDto>>;
