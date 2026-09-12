using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seeding;

public class InitialAdminSeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public InitialAdminSeeder(ApplicationDbContext dbContext, IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync(string name, string email, string password)
    {
        var adminRole = await _dbContext.Roles.SingleOrDefaultAsync(r => r.Name == "Admin");

        if (adminRole is null)
        {
            throw new InvalidOperationException("Admin role does not exist. Seed roles first.");
        }

        email = email.Trim().ToLowerInvariant();
        name = name.Trim();

        // Make sure the new email isn't already used
        // by some other non-admin user.
        var emailOwner = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        var admin = await _dbContext.Users.Include(u => u.Sessions).FirstOrDefaultAsync(u => u.RoleId == adminRole.Id);
        var now = DateTimeOffset.UtcNow;
        // --------------------------------------------------
        // First run: create Admin
        // --------------------------------------------------

        if (admin is null)
        {
            if (emailOwner is not null)
            {
                throw new InvalidOperationException("That email is already being used by another user.");
            }

            admin = new User
            {
                Name = name,
                Email = email,
                RoleId = adminRole.Id,

                EmailConfirmed = true,
                Status = UserStatus.Active,

                OtpAttempts = 0,
                FailedLoginAttempts = 0,

                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };

            admin.PasswordHash = _passwordHasher.HashPassword(admin, password);
            _dbContext.Users.Add(admin);
        }

        // --------------------------------------------------
        // Later run: replace/reset Admin credentials
        // --------------------------------------------------

        else
        {
            if (emailOwner is not null && emailOwner.Id != admin.Id)
            {
                throw new InvalidOperationException("That email is already being used by another user.");
            }

            admin.Name = name;
            admin.Email = email;

            admin.PasswordHash = _passwordHasher.HashPassword(admin, password);

            admin.EmailConfirmed = true;
            admin.Status = UserStatus.Active;

            // Clear OTP state
            admin.OtpHash = null;
            admin.OtpExpiresAtUtc = null;
            admin.OtpAttempts = 0;

            // Clear login lockout
            admin.FailedLoginAttempts = 0;
            admin.LockoutUntilUtc = null;

            admin.UpdatedAtUtc = now;

            // Revoke existing login sessions
            foreach (var session in admin.Sessions)
            {
                if (session.RevokedAtUtc is null)
                {
                    session.RevokedAtUtc = now;
                    session.RevocationReason =  "Administrator credentials were reset.";
                }
            }
        }
        await _dbContext.SaveChangesAsync();
    }
}