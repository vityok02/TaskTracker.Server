using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Authentication.ResetPassword;

internal sealed class ResetPasswordCommandHandler
    : ICommandHandler<ResetPasswordCommand, ResetPasswordDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IUserManager _userManager;

    public ResetPasswordCommandHandler(
        IUserRepository userRepository,
        IEmailService emailService,
        IUserManager userManager)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _userManager = userManager;
    }

    public async Task<Result<ResetPasswordDto>> Handle(
        ResetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetByEmailAsync(command.Email);

        if (user is null)
        {
            return Result<ResetPasswordDto>
                .Failure(UserErrors.NotFound);
        }

        var resetTokenValue = _userManager
            .GeneratePasswordResetToken(user.Id);

        // TODO: replace url into appsettings.

        await _emailService.SendEmailAsync(
            user.Email,
            "Reset Password",
            $"Redirect to the url to reset your password: https://localhost:7001/reset-password/{resetTokenValue}",
            cancellationToken);

        return Result<ResetPasswordDto>.Success(
            new ResetPasswordDto(user.Email, resetTokenValue));
    }
}