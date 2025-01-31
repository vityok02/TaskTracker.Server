using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Abstract;
using Domain.Errors;

namespace Application.Modules.Users.Identity.Login;

internal sealed class LoginCommandHandler
    : ICommandHandler<LoginCommand, string>
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

    public async Task<Result<string>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var loginRequest = command.LoginRequest;

        var user = await _userRepository.GetByEmailAsync(loginRequest.Email);

        if (user is null)
        {
            return Result<string>.Failure(UserErrors.InvalidCredentials);
        }

        bool verified = _passwordHasher.Verify(loginRequest.Password, user.Password);

        if (!verified)
        {
            return Result<string>.Failure(UserErrors.InvalidCredentials);
        }

        string token = _jwtProvider.Generate(user);

        return token;
    }
}