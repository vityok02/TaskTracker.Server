using Application.Abstract.Messaging;

namespace Application.Modules.Users.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId)
    : IQuery<UserDto>;
