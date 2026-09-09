using Domain.Enums;

namespace Domain.Entities;

public class AuditLog
{
    public long Id { get; set; }

    // Who performed the action?
    public long? ActorUserId { get; set; }

    public User? ActorUser { get; set; }

    // Which login session was used?
    public long? SessionId { get; set; }

    public Session? Session { get; set; }

    public AuditAction Action { get; set; }

    // Main affected entity
    public string EntityType { get; set; } = string.Empty;

    public string EntityId { get; set; } = string.Empty;

    // Optional related entity
    public string? RelatedEntityType { get; set; }
    public string? RelatedEntityId { get; set; }

    // Can later contain JSON
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }

    public string? Description { get; set; }


    public string? IpAddress { get; set; }
    public string? CorrelationId { get; set; }

    public DateTimeOffset OccurredAtUtc { get; set; }
}