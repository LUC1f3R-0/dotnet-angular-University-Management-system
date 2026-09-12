using Domain.Entities;
using Infrastructure.Email;
using Infrastructure.Initialization;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddEmail();
        services.AddScoped<StartupInitializer>();
        services.AddScoped<RoleSeeder>();
        services.AddScoped<InitialAdminSeeder>();
        services.AddScoped<DatabaseSeeder>();
        
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        return services;
    }
}

