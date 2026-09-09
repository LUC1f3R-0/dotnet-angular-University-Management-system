using Domain.Enums;

namespace Domain.Entities;

public class SecurityEvent
{
    public long Id { get; set; }

    // Can be null, for example:
    // login attempt using an email that doesn't exist
    public long? UserId { get; set; }

    public User? User { get; set; }

    // Some events happen before a session exists
    public long? SessionId { get; set; }

    public Session? Session { get; set; }

    public SecurityEventType EventType { get; set; }

    public bool Success { get; set; }

    public string? FailureReason { get; set; }


    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public DateTimeOffset OccurredAtUtc { get; set; }
}