using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Shared;

namespace Application.Modules.Users.GetAllUsers;

public sealed record GetAllUsersQuery()
    : IQuery<IEnumerable<UserResponse>>;

internal sealed class GetAllUsersQueryHandler
    : IQueryHandler<GetAllUsersQuery, IEnumerable<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<IEnumerable<UserResponse>>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _userRepository
            .GetAllAsync();

        return users
            .Select(u => new UserResponse(u.Id, u.UserName, u.Email))
            .ToArray();
    }
}
