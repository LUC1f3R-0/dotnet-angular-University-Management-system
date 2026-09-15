namespace Application.Authentication.Models;

public sealed record AccessTokenResult(string Token, DateTimeOffset ExpiresAtUtc);