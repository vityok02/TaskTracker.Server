using Domain.Entities;
using Domain.Shared;

namespace Application.Abstract.Interfaces;

public interface IUserManager
{
    string GeneratePasswordResetToken(Guid userId);
    Task<Result> ResetPasswordAsync(UserEntity user, string token, string password);
    Task<Result> ChangePasswordAsync(UserEntity user, string currentPassword, string newPassword);
}
