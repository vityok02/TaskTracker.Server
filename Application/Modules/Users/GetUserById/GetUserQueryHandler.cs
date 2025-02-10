using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Users.GetUserById;

internal sealed class GetUserQueryHandler
    : IQueryHandler<GetUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> Handle(
        GetUserQuery query,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId);

        return user is null
            ? Result<UserResponse>
                .Failure(UserErrors.NotFound)
            : Result<UserResponse>
                .Success(new UserResponse(user.Id, user.UserName, user.Email));
    }
}