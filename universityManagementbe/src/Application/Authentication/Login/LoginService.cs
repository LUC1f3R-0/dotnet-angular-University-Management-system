using Application.Authentication.Abstractions;
using Application.Exceptions;
using Domain.Entities;
using Domain.Enums;

namespace Application.Authentication.Login;

public sealed class LoginService : ILoginService
{
    private readonly IUserRepository _userRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public LoginService(IUserRepository userRepository, ISessionRepository sessionRepository, IPasswordService passwordService, ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginResult> LoginAsync(string email, string password, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        var user = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedException("Email or password is incorrect.");
        }

        if (user.Status != UserStatus.Active)
        {
            throw new ForbiddenException("This account is not active.");
        }

        var now = DateTimeOffset.UtcNow;

        if (user.LockoutUntilUtc.HasValue && user.LockoutUntilUtc.Value > now)
        {
            throw new UnauthorizedException("This account is temporarily locked.");
        }

        var passwordValid =_passwordService.VerifyPassword(user, password);

        if (!passwordValid)
        {
            user.FailedLoginAttempts++;
            user.UpdatedAtUtc = now;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            throw new UnauthorizedException("Email or password is incorrect.");
        }
        
        user.FailedLoginAttempts = 0;
        user.LockoutUntilUtc = null;
        user.UpdatedAtUtc = now;

        var refreshToken =_tokenService.CreateRefreshToken();

        var session = new Session
        {
            SessionUuid = Guid.NewGuid(),
            UserId = user.Id,

            RefreshTokenHash = refreshToken.Hash,

            CreatedAtUtc = now,
            LastSeenAtUtc = now,
            ExpiresAtUtc = refreshToken.ExpiresAtUtc,

            IpAddress = ipAddress,
            UserAgent = userAgent
        };

        await _sessionRepository.AddAsync(session, cancellationToken);

        var accessToken = _tokenService.CreateAccessToken(user, session);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResult(
            AccessToken: accessToken.Token,
            AccessTokenExpiresAtUtc: accessToken.ExpiresAtUtc,
            RefreshToken: refreshToken.Token,
            RefreshTokenExpiresAtUtc: refreshToken.ExpiresAtUtc,
            UserUuid: user.Uuid,
            Name: user.Name,
            Email: user.Email,
            Role: user.Role.Name);
    }
}