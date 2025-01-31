using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Abstract;
using Domain.Entities;
using Domain.Errors;

namespace Application.Modules.Users.Identity.RegisterUser;

internal sealed class RegisterCommandHandler
    : ICommandHandler<RegisterCommand, UserResponse>
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

    public async Task<Result<UserResponse>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var userExistsWithSuchEmail = await _userRepository
            .ExistsByEmailAsync(command.UserDto.Email);

        if (userExistsWithSuchEmail)
        {
            return Result<UserResponse>
                .Failure(UserErrors.AlreadyExists);
        }

        if (!command.UserDto.IsPasswordsMatch)
        {
            return Result<UserResponse>
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

        return Result<UserResponse>
            .Success(new RegisterUserResponse(
                id,
                user.UserName,
                user.Email,
                token));
    }
}