using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Identity.ChangePassword;

internal sealed class ChangePasswordCommandHandler
    : ICommandHandler<ChangePasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManager _userManager;

    public ChangePasswordCommandHandler(
        IUserRepository userRepository,
        IUserManager userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<Result> Handle(
        ChangePasswordCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetByIdAsync(command.UserId);

        if (user is null)
        {
            return Result
                .Failure(UserErrors.NotFound);
        }
        
        var result = await _userManager.ChangePasswordAsync(
            user, command.CurrentPassword, command.NewPassword);

        if (result.IsFailure)
        {
            return Result
                .Failure(result.Error);
        }

        return Result.Success();
    }
}
