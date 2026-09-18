using Application.Authentication.Abstractions;
using Application.Exceptions;
using Domain.Enums;

namespace Application.Authentication.Refresh;

public sealed class RefreshService : IRefreshService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshService(ISessionRepository sessionRepository, ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<RefreshResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new UnauthorizedException("Refresh token is missing.");
        }

        var refreshTokenHash = _tokenService.HashRefreshToken(refreshToken);
        var session = await _sessionRepository.GetByRefreshTokenHashAsync(refreshTokenHash, cancellationToken);
        if (session is null)
        {
            throw new UnauthorizedException("Invalid refresh token.");
        }
        
        var now = DateTimeOffset.UtcNow;
        
        if (session.RevokedAtUtc is not null)
        {
            throw new UnauthorizedException("Session has been revoked.");
        }

        if (session.ExpiresAtUtc <= now)
        {
            throw new UnauthorizedException("Session has expired.");
        }

        if (session.User.Status != UserStatus.Active)
        {
            throw new UnauthorizedException("Account is not active.");
        }

        // Rotate refresh token
        var newRefreshToken = _tokenService.CreateRefreshToken();
        session.RefreshTokenHash = newRefreshToken.Hash;
        session.LastSeenAtUtc = now;

        // We keep the original Session expiry.
        // Refreshing does not make the session live forever.

        var accessToken = _tokenService.CreateAccessToken(session.User, session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return
        new RefreshResult(
            AccessToken: accessToken.Token,
            AccessTokenExpiresAtUtc: accessToken.ExpiresAtUtc,
            RefreshToken: newRefreshToken.Token,
            RefreshTokenExpiresAtUtc: session.ExpiresAtUtc);
    }
}