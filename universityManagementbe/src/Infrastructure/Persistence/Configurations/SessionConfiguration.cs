using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("sessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
        .ValueGeneratedOnAdd();

        // Public session identifier
        builder.Property(s => s.SessionUuid)
        .HasDefaultValueSql("gen_random_uuid()")
        .ValueGeneratedOnAdd()
        .IsRequired();

        builder.HasIndex(s => s.SessionUuid)
        .IsUnique();

        // User relationship
        builder.Property(s => s.UserId)
        .IsRequired();

        builder.HasOne(s => s.User)
        .WithMany(u => u.Sessions)
        .HasForeignKey(s => s.UserId)
        .OnDelete(DeleteBehavior.Restrict);

        // Refresh token
        builder.Property(s => s.RefreshTokenHash)
        .HasMaxLength(500)
        .IsRequired();

        // Session lifecycle
        builder.Property(s => s.CreatedAtUtc)
        .IsRequired();

        builder.Property(s => s.LastSeenAtUtc)
        .IsRequired();

        builder.Property(s => s.ExpiresAtUtc)
        .IsRequired();

        builder.Property(s => s.RevokedAtUtc);

        // Client information
        builder.Property(s => s.IpAddress)
        .HasMaxLength(45);

        builder.Property(s => s.UserAgent)
        .HasMaxLength(1000);

        builder.Property(s => s.Browser)
        .HasMaxLength(100);

        builder.Property(s => s.OperatingSystem)
        .HasMaxLength(100);

        builder.Property(s => s.DeviceName)
        .HasMaxLength(150);

        builder.Property(s => s.RevocationReason)
        .HasMaxLength(255);

        // Useful indexes
        builder.HasIndex(s => s.UserId);
        builder.HasIndex(s => s.ExpiresAtUtc);
        builder.HasIndex(s => s.RevokedAtUtc);
    }
}