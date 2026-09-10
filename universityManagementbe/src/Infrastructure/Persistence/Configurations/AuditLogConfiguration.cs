using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
        .ValueGeneratedOnAdd();

        // Actor relationship
        builder.HasOne(a => a.ActorUser)
        .WithMany(u => u.AuditLogs)
        .HasForeignKey(a => a.ActorUserId)
        .OnDelete(DeleteBehavior.Restrict);

        // Session relationship
        builder.HasOne(a => a.Session)
        .WithMany(s => s.AuditLogs)
        .HasForeignKey(a => a.SessionId)
        .OnDelete(DeleteBehavior.Restrict);

        // Action
        builder.Property(a => a.Action)
        .HasConversion<string>()
        .HasMaxLength(50)
        .IsRequired();

        // Main affected entity
        builder.Property(a => a.EntityType)
        .HasMaxLength(100)
        .IsRequired();

        builder.Property(a => a.EntityId)
        .HasMaxLength(100)
        .IsRequired();


        // Related entity
        builder.Property(a => a.RelatedEntityType)
        .HasMaxLength(100);

        builder.Property(a => a.RelatedEntityId)
        .HasMaxLength(100);

        // Before / after values
        builder.Property(a => a.OldValues);
        builder.Property(a => a.NewValues);

        builder.Property(a => a.Description)
        .HasMaxLength(1000);

        builder.Property(a => a.IpAddress)
        .HasMaxLength(45);

        builder.Property(a => a.CorrelationId)
        .HasMaxLength(100);

        builder.Property(a => a.OccurredAtUtc)
        .IsRequired();

        // Useful indexes
        builder.HasIndex(a => a.ActorUserId);
        builder.HasIndex(a => a.SessionId);
        builder.HasIndex(a => a.Action);
        builder.HasIndex(a => a.OccurredAtUtc);
        builder.HasIndex(a => a.CorrelationId);
    }
}