using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;

namespace Infrastructure.Services;

public class UserManager : IUserManager
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IResetTokenService _resetTokenService;

    public UserManager(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IResetTokenService resetTokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _resetTokenService = resetTokenService;
    }

    public async Task<Result> ChangePasswordAsync(
        UserEntity user,
        string currentPassword,
        string newPassword)
    {
        var isVerified = _passwordHasher
            .Verify(currentPassword, user.Password);

        if (!isVerified)
        {
            return Result
                .Failure(UserErrors.InvalidCredentials);
        }

        var hashedPassword = _passwordHasher
            .Hash(newPassword);

        user.Password = hashedPassword;

        await _userRepository
            .UpdateAsync(user);

        return Result.Success();
    }

    public string GeneratePasswordResetToken(Guid userId)
    {
        return _resetTokenService
            .GenerateToken(userId.ToString());
    }

    public async Task<Result> ResetPasswordAsync(
        UserEntity user,
        string token,
        string password)
    {
        var hashedPassword = _passwordHasher
            .Hash(password);

        user.Password = hashedPassword;

        await _userRepository
            .UpdateAsync(user);

        return Result.Success();
    }
}
