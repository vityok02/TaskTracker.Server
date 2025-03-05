using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Users.SearchUserQuery;

internal sealed class SearchUsersQueryHandler
    : IQueryHandler<SearchUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public SearchUsersQueryHandler(
        IUserRepository userRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<UserDto>>> Handle(
        SearchUsersQuery query,
        CancellationToken cancellationToken)
    {
        var users = await _userRepository
            .GetByNameContainsAsync(query.Username);

        if (users is null)
        {
            return Result<IEnumerable<UserDto>>
                .Failure(UserErrors.NotFound);
        }

        return Result<IEnumerable<UserDto>>
            .Success(_mapper.Map<IEnumerable<UserDto>>(users));
    }
}
