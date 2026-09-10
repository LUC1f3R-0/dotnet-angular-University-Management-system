using Domain.Enums;

namespace Domain.Entities;

public class User
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // Role
    public long RoleId { get; set; }
    public Role Role { get; set; } = null!;

    // Account state
    public bool EmailConfirmed { get; set; }
    public UserStatus Status { get; set; }

    // OTP
    public string? OtpHash { get; set; }
    public DateTimeOffset? OtpExpiresAtUtc { get; set; }
    public int OtpAttempts { get; set; }

    // Login security
    public int FailedLoginAttempts { get; set; }
    public DateTimeOffset? LockoutUntilUtc { get; set; }

    // Lifecycle
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }

    // Relationships
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public ICollection<SecurityEvent> SecurityEvents { get; set; } = new List<SecurityEvent>();
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}