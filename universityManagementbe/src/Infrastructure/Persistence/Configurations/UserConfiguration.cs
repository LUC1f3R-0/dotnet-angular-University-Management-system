using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        // Primary key
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
        .ValueGeneratedOnAdd();

        // Public UUID
        builder.Property(u => u.Uuid)
        .HasDefaultValueSql("gen_random_uuid()")
        .ValueGeneratedOnAdd()
        .IsRequired();

        builder.HasIndex(u => u.Uuid)
        .IsUnique();

        // Identity
        builder.Property(u => u.Name)
        .HasMaxLength(100)
        .IsRequired();

        builder.Property(u => u.Email)
        .HasMaxLength(255)
        .IsRequired();

        builder.HasIndex(u => u.Email)
        .IsUnique();

        // Authentication
        builder.Property(u => u.PasswordHash)
        .HasMaxLength(500)
        .IsRequired();

        // Role relationship
        builder.Property(u => u.RoleId)
        .IsRequired();

        builder.HasOne(u => u.Role)
        .WithMany(r => r.Users)
        .HasForeignKey(u => u.RoleId)
        .OnDelete(DeleteBehavior.Restrict);

        // Account state
        builder.Property(u => u.EmailConfirmed)
        .HasDefaultValue(false);

        builder.Property(u => u.Status)
        .HasConversion<string>()
        .HasMaxLength(30)
        .IsRequired();

        // OTP
        builder.Property(u => u.OtpHash)
        .HasMaxLength(500);

        builder.Property(u => u.OtpExpiresAtUtc);

        builder.Property(u => u.OtpAttempts)
        .HasDefaultValue(0);

        // Login security
        builder.Property(u => u.FailedLoginAttempts)
        .HasDefaultValue(0);

        builder.Property(u => u.LockoutUntilUtc);

        // Lifecycle
        builder.Property(u => u.CreatedAtUtc)
        .IsRequired();

        builder.Property(u => u.UpdatedAtUtc)
        .IsRequired();
    }
}