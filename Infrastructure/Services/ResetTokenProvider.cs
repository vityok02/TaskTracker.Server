using Application.Abstract.Interfaces;
using Domain.Errors;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;

public class ResetTokenProvider : IResetTokenService
{
    // TODO: get secret key from configuration
    private const string SecretKey = "140ujgsiotu38w09gxlzksahgsggss123m";

    private readonly ILogger<ResetTokenProvider> _logger;

    public ResetTokenProvider(ILogger<ResetTokenProvider> logger)
    {
        _logger = logger;
    }

    public string GenerateToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8
            .GetBytes(SecretKey);

        var claims = new[]
        {
            new Claim("userId", userId)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow
                .AddMinutes(10),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler
            .CreateToken(tokenDescriptor);

        return tokenHandler
            .WriteToken(token);
    }

    public async Task<Result<Guid>> TryGetUserIdAsync(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8
            .GetBytes(SecretKey);

        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = await tokenHandler
                .ValidateTokenAsync(token, parameters);

            var userIdClaim = principal.ClaimsIdentity
                .FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim)
                || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Result<Guid>
                    .Failure(UserErrors.InvalidToken);
            }

            return Result<Guid>
                .Success(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "An error occurred while validating token: {Token}",
                token);

            return Result<Guid>
                .Failure(UserErrors.InvalidToken);
        }
    }
}
