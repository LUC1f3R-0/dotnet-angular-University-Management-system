using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Authentication.Abstractions;
using Application.Authentication.Models;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public sealed class JwtTokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AccessTokenResult CreateAccessToken(User user,Session session)
    {
        var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is missing.");
        var issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer is missing.");
        var audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT audience is missing.");
        var minutesText = _configuration["Jwt:AccessTokenMinutes"];
        if (!int.TryParse(minutesText, out var accessTokenMinutes))
        {
            throw new InvalidOperationException("Jwt:AccessTokenMinutes is invalid.");
        }

        var now = DateTimeOffset.UtcNow;

        var expiresAt = now.AddMinutes(accessTokenMinutes);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,user.Uuid.ToString()),
            new(ClaimTypes.Role,user.Role.Name),
            new("sid", session.SessionUuid.ToString())
        };
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new AccessTokenResult(tokenString, expiresAt);
    }


    public RefreshTokenResult CreateRefreshToken()
    {
        var daysText = _configuration["Jwt:RefreshTokenDays"];
        if (!int.TryParse(daysText, out var refreshTokenDays))
        {
            throw new InvalidOperationException("Jwt:RefreshTokenDays is invalid.");
        }

        var bytes = RandomNumberGenerator.GetBytes(64);
        var token = Convert.ToHexString(bytes);
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        var hash = Convert.ToHexString(hashBytes);
        var expiresAt = DateTimeOffset.UtcNow.AddDays(refreshTokenDays);
        
        return new RefreshTokenResult(token,hash,expiresAt);
    }
}