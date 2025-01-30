using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Abstract;

namespace Application.Users.GetUser;

internal sealed class GetUserQueryHandler
    : IQueryHandler<GetUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<UserResponse>> Handle(
        GetUserQuery query,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId);

        return user is null
            ? Result<UserResponse>.Failure("User.NotFound", "User not found")
            : Result<UserResponse>.Success(_mapper.Map<UserResponse>(user));
    }
}