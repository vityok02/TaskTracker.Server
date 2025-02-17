using Domain.Shared;

namespace Application.Abstract.Interfaces;

public interface IResetTokenService
{
    string GenerateToken(string userId);

    Task<Result<Guid>> TryGetUserIdAsync(string token);
}