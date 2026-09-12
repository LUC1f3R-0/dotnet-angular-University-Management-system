namespace Infrastructure.Persistence.Seeding;

public class DatabaseSeeder
{
    private readonly RoleSeeder _roleSeeder;
    private readonly InitialAdminSeeder _initialAdminSeeder;

    public DatabaseSeeder(RoleSeeder roleSeeder, InitialAdminSeeder initialAdminSeeder)
    {
        _roleSeeder = roleSeeder;
        _initialAdminSeeder = initialAdminSeeder;
    }

    public async Task SeedAsync(string adminName, string adminEmail, string adminPassword)
    {
        await _roleSeeder.SeedAsync();
        await _initialAdminSeeder.SeedAsync(adminName, adminEmail, adminPassword);
    }
}