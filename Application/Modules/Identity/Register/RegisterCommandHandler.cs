using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Application.Modules.Users.Identity.RegisterUser;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Identity.Register;

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
            .IsEmailUniqueAsync(command.Email);

        if (userExistsWithSuchEmail)
        {
            return Result<RegisterResponse>
                .Failure(UserErrors.AlreadyExists);
        }

        var hashedPassword = _passwordHasher
            .Hash(command.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = command.UserName,
            Email = command.Email,
            Password = hashedPassword
        };

        var id = await _userRepository
            .CreateAsync(user);

        var token = _jwtProvider
            .Generate(user);

        return new RegisterResponse(id, token);
    }
}