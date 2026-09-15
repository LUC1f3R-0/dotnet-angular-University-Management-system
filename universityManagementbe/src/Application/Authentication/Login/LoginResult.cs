namespace Application.Authentication.Login;

public sealed record LoginResult(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAtUtc,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAtUtc,
    Guid UserUuid,
    string Name,
    string Email,
    string Role
);