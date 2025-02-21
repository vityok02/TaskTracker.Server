using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Authentication.Login;

internal sealed class LoginCommandHandler
    : ICommandHandler<LoginCommand, TokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IJwtProvider jwtProvider,
        IPasswordHasher passwordHasher,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<Result<TokenDto>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetByEmailAsync(command.Email);

        if (user is null)
        {
            return UserErrors.InvalidCredentials;
        }

        bool verified = _passwordHasher
            .Verify(command.Password, user.Password);

        if (!verified)
        {
            return UserErrors.InvalidCredentials;
        }

        var token = _jwtProvider.Generate(user);

        return _mapper.Map<TokenDto>(token);
    }
}