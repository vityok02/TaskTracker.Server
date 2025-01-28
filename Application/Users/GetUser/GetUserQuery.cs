using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Abstract;

namespace Application.Users.GetUser;

public record GetUserQuery(Guid UserId) : IQuery<UserResponse>;

internal sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId);

        return user is null
            ? Result<UserResponse>.Failure("User.NotFound", "User not found")
            : Result<UserResponse>.Success(new UserResponse(user.Id, user.UserName, user.Email));
    }
}