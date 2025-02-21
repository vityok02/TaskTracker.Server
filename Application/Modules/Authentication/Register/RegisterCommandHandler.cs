using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Authentication.Register;

internal sealed class RegisterCommandHandler
    : ICommandHandler<RegisterCommand, RegisterDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
    }

    public async Task<Result<RegisterDto>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository
            .GetUserByEmailOrNameAsync(command.Email, command.Username);

        if (existingUser is not null)
        {
            return existingUser.Email == command.Email
                ? Result<RegisterDto>
                    .Failure(UserErrors.EmailAlreadyExists)
                : Result<RegisterDto>
                    .Failure(UserErrors.NameAlreadyExists);
        }

        var hashedPassword = _passwordHasher
            .Hash(command.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = command.Username,
            Email = command.Email,
            Password = hashedPassword
        };

        var id = await _userRepository
            .CreateAsync(user);

        var token = _jwtProvider
            .Generate(user);

        return new RegisterDto(id, _mapper.Map<TokenDto>(token));
    }
}