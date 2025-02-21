using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Users.GetUserById;

internal sealed class GetUserQueryHandler
    : IQueryHandler<GetUserQuery, UserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto>> Handle(
        GetUserQuery query,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId);

        return user is null
            ? Result<UserDto>
                .Failure(UserErrors.NotFound)
            : Result<UserDto>
                .Success(new UserDto(user.Id, user.UserName, user.Email));
    }
}