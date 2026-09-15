using Application.Authentication.Abstractions;
using Domain.Entities;
using Infrastructure.Authentication;
using Infrastructure.Email;
using Infrastructure.Initialization;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Persistence
        services.AddPersistence(configuration);

        // Email
        services.AddEmail();

        // Startup
        services.AddScoped<StartupInitializer>();

        // Seeding
        services.AddScoped<RoleSeeder>();
        services.AddScoped<InitialAdminSeeder>();
        services.AddScoped<DatabaseSeeder>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Authentication
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ITokenService, JwtTokenService>();

        return services;
    }
}

