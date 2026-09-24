namespace Infrastructure.Options;

public sealed class RateLimitingOptions
{
    public const string SectionName = "RateLimiting";

    public int LoginPermitLimit { get; set; } = 5;
    public int LoginWindowSeconds { get; set; } = 60;

    public int RefreshPermitLimit { get; set; } = 10;
    public int RefreshWindowSeconds { get; set; } = 60;
}
