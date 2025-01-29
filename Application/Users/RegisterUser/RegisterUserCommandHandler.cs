using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Application.Users.GetUser;
using Domain.Abstract;
using Domain.Entities;
using Domain.Errors;

namespace Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler
    : ICommandHandler<RegisterUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<UserResponse>> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        if (!command.UserDto.IsPasswordsMatch)
        {
            return Result<UserResponse>.Failure(UserErrors.PasswordsDoNotMatch);
        }

        var hashedPassword = _passwordHasher.Hash(command.UserDto.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = command.UserDto.UserName,
            Email = command.UserDto.Email,
            Password = hashedPassword
        };

        var id = await _userRepository.CreateAsync(user);

        return Result<UserResponse>.Success(new UserResponse(id, user.UserName, user.Email));
    }
}