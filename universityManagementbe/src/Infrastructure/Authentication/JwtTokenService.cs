using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Authentication.Abstractions;
using Application.Authentication.Models;
using Application.Exceptions;
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

    public AccessTokenResult CreateAccessToken(User user, Session session)
    {
        var key = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidRequestOperationException("Jwt:Key is missing.");
        }

        if (string.IsNullOrWhiteSpace(issuer))
        {
            throw new InvalidRequestOperationException("Jwt:Issuer is missing.");
        }

        if (string.IsNullOrWhiteSpace(audience))
        {
            throw new InvalidRequestOperationException("Jwt:Audience is missing.");
        }

        if (!int.TryParse(_configuration["Jwt:AccessTokenMinutes"], out var accessTokenMinutes))
        {
            throw new InvalidRequestOperationException("Jwt:AccessTokenMinutes is invalid.");
        }

        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.AddMinutes(accessTokenMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Uuid.ToString()),
            new("role", user.Role.Name),
            new("sid", session.SessionUuid.ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token =
        new JwtSecurityToken(
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
        if (!int.TryParse(_configuration["Jwt:RefreshTokenDays"], out var refreshTokenDays))
        {
            throw new InvalidRequestOperationException("Jwt:RefreshTokenDays is invalid.");
        }

        var randomBytes = RandomNumberGenerator.GetBytes(64);
        var token = Convert.ToHexString(randomBytes);
        var hash = HashRefreshToken(token);
        var expiresAt = DateTimeOffset.UtcNow.AddDays(refreshTokenDays);

        return new RefreshTokenResult(token, hash, expiresAt);
    }

    public string HashRefreshToken(string refreshToken)
    {
        var bytes = Encoding.UTF8.GetBytes(refreshToken);
        var hashBytes = SHA256.HashData(bytes);

        return Convert.ToHexString(hashBytes);
    }
}
