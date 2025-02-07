using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Abstract;
using Domain.Entities;
using Domain.Errors;

namespace Application.Modules.Users.Identity.RegisterUser;

internal sealed class RegisterCommandHandler
    : ICommandHandler<RegisterCommand, RegisterResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<RegisterResponse>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var userExistsWithSuchEmail = await _userRepository
            .ExistsByEmailAsync(command.UserDto.Email);

        if (userExistsWithSuchEmail)
        {
            return Result<RegisterResponse>
                .Failure(UserErrors.AlreadyExists);
        }

        if (!command.UserDto.IsPasswordsMatch())
        {
            return Result<RegisterResponse>
                .Failure(UserErrors.PasswordsDoNotMatch);
        }

        var hashedPassword = _passwordHasher
            .Hash(command.UserDto.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = command.UserDto.UserName,
            Email = command.UserDto.Email,
            Password = hashedPassword
        };

        var id = await _userRepository
            .CreateAsync(user);

        var token = _jwtProvider
            .Generate(user);

        return new RegisterResponse(id, token);
    }
}