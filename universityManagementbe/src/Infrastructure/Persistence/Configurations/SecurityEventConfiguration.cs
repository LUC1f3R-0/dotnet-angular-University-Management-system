using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SecurityEventConfiguration: IEntityTypeConfiguration<SecurityEvent>
{
    public void Configure(EntityTypeBuilder<SecurityEvent> builder)
    {
        builder.ToTable("security_events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
        .ValueGeneratedOnAdd();

        // User relationship
        builder.HasOne(e => e.User)
        .WithMany(u => u.SecurityEvents)
        .HasForeignKey(e => e.UserId)
        .OnDelete(DeleteBehavior.Restrict);
        
        // Session relationship
        builder.HasOne(e => e.Session)
        .WithMany(s => s.SecurityEvents)
        .HasForeignKey(e => e.SessionId)
        .OnDelete(DeleteBehavior.Restrict);

        // Event information
        builder.Property(e => e.EventType)
        .HasConversion<string>()
        .HasMaxLength(50)
        .IsRequired();

        builder.Property(e => e.Success)
        .IsRequired();

        builder.Property(e => e.FailureReason)
        .HasMaxLength(500);

        // Client information
        builder.Property(e => e.IpAddress)
        .HasMaxLength(45);

        builder.Property(e => e.UserAgent)
        .HasMaxLength(1000);

        // Lifecycle
        builder.Property(e => e.OccurredAtUtc)
        .IsRequired();

        // Useful indexes
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.SessionId);
        builder.HasIndex(e => e.EventType);
        builder.HasIndex(e => e.OccurredAtUtc);
    }
}