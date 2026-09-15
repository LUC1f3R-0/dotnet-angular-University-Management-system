namespace Application.Authentication.Models;

public sealed record RefreshTokenResult(
    string Token,
    string Hash,
    DateTimeOffset ExpiresAtUtc
);