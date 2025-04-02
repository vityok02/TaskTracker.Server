using Application.Abstract.Interfaces;
using Application.Modules.Authentication;
using Domain.Entities;
using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Authentication;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;
    private static TimeSpan ExpiresIn => TimeSpan.FromHours(24);

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public TokenModel Generate(UserEntity user)
    {
        var claims = new Claim[]
        {
            new(ClaimTypes.Name, user.UserName),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Uri, user.AvatarUrl ?? string.Empty)
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.Now.Add(ExpiresIn),
            signingCredentials);

        string tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return new(tokenValue, ExpiresIn.TotalHours);
    }
}
