using Application.Authentication.Models;
using Domain.Entities;

namespace Application.Authentication.Abstractions;

public interface ITokenService
{
    AccessTokenResult CreateAccessToken(User user, Session session);
    RefreshTokenResult CreateRefreshToken();
    string HashRefreshToken(string refreshToken);
}