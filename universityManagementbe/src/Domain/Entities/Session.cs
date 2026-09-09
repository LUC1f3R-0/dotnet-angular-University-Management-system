namespace Domain.Entities;

public class Session
{
    public long Id { get; set; }
    public Guid SessionUuid { get; set; }

    // Owner
    public long UserId { get; set; }
    public User User { get; set; } = null!;

    // Refresh token
    public string RefreshTokenHash { get; set; } = string.Empty;

    // Session lifecycle
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset LastSeenAtUtc { get; set; }
    public DateTimeOffset ExpiresAtUtc { get; set; }
    public DateTimeOffset? RevokedAtUtc { get; set; }

    // Client information
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? Browser { get; set; }
    public string? OperatingSystem { get; set; }
    public string? DeviceName { get; set; }

    public string? RevocationReason { get; set; }

    public ICollection<SecurityEvent> SecurityEvents { get; set; } = new List<SecurityEvent>();

    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}