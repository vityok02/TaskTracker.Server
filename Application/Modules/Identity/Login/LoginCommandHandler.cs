using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Identity.Login;

internal sealed class LoginCommandHandler
    : ICommandHandler<LoginCommand, TokenResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IJwtProvider jwtProvider,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<TokenResponse>> Handle(
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

        return token;
    }
}