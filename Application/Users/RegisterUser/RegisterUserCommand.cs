using Api.Users.Dtos;
using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Application.Users.GetUser;
using Domain;
using Domain.Abstract;

namespace Application.Users.RegisterUser;

public record RegisterUserCommand(RegisterUserRequest UserDto)
    : ICommand<UserResponse>;

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

        var user = User
            .Create(Guid.NewGuid(), command.UserDto.UserName, command.UserDto.Email, hashedPassword);

        var id = await _userRepository.CreateAsync(user);

        return Result<UserResponse>.Success(new UserResponse(id, user.UserName, user.Email));
    }
}