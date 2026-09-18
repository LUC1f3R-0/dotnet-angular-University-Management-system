using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seeding;

public class RoleSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public RoleSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync()
    {
        string[] roleNames =
        {
            "Admin",
            "SectionCoordinator",
            "AcademicAdvisor",
            "Lecturer",
            "Student"
        };

        foreach (var roleName in roleNames)
        {
            var exists = await _dbContext.Roles.AnyAsync(r => r.Name == roleName);

            if (exists)
            continue;

            var role = new Role
            {
                Name = roleName, CreatedAtUtc = DateTimeOffset.UtcNow
            };

            _dbContext.Roles.Add(role);
        }

        await _dbContext.SaveChangesAsync();
    }
}