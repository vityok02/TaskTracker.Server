using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Identity.SetPassword;

internal sealed class SetPasswordCommandHandler
    : ICommandHandler<SetPasswordCommand, TokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IResetTokenService _resetTokenService;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUserManager _userManager;

    public SetPasswordCommandHandler(
        IUserRepository userRepository,
        IResetTokenService resetTokenService,
        IJwtProvider jwtProvider,
        IUserManager userManager)
    {
        _userRepository = userRepository;
        _resetTokenService = resetTokenService;
        _jwtProvider = jwtProvider;
        _userManager = userManager;
    }

    public async Task<Result<TokenDto>> Handle(
        SetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var userIdResult = await _resetTokenService
            .TryGetUserIdAsync(command.ResetToken);

        if (userIdResult.IsFailure)
        {
            return Result<TokenDto>
                .Failure(userIdResult.Error);
        }

        var user = await _userRepository
            .GetByIdAsync(userIdResult.Value);

        if (user is null)
        {
            return Result<TokenDto>
                .Failure(UserErrors.NotFound);
        }

        await _userManager.ResetPasswordAsync(
            user, command.ResetToken, command.Password);

        var token = _jwtProvider
            .Generate(user);

        return Result<TokenDto>
            .Success(new TokenDto(token.Token, token.ExpiresIn));
    }
}
