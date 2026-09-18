using Application.Authentication.Abstractions;

namespace Application.Authentication.Logout;

public sealed class LogoutService : ILogoutService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutService(ISessionRepository sessionRepository, ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task LogoutAsync(string? refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return;
        }

        var hash = _tokenService.HashRefreshToken(refreshToken);

        var session =await _sessionRepository.GetByRefreshTokenHashAsync(hash, cancellationToken);

        if (session is null)
        {
            return;
        }

        if (session.RevokedAtUtc is null)
        {
            session.RevokedAtUtc = DateTimeOffset.UtcNow;
            session.RevocationReason = "User logged out.";

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}