using Application.Abstract.Messaging;

namespace Application.Modules.Users.GetUserById;

public sealed record GetUserQuery(Guid UserId)
    : IQuery<UserResponse>;
