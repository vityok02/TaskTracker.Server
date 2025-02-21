using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Shared;

namespace Application.Modules.Users.GetAllUsers;

public sealed record GetAllUsersQuery()
    : IQuery<IEnumerable<UserDto>>;

internal sealed class GetAllUsersQueryHandler
    : IQueryHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<IEnumerable<UserDto>>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _userRepository
            .GetAllAsync();

        return users
            .Select(u => new UserDto(u.Id, u.UserName, u.Email))
            .ToArray();
    }
}
