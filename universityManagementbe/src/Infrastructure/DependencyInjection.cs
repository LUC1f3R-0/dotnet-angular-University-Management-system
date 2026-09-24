using Application.Authentication.Abstractions;
using Domain.Entities;
using Infrastructure.Authentication;
using Infrastructure.Email;
using Infrastructure.Initialization;
using Infrastructure.Options;
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

        // Options bindings
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.SectionName));
        services.Configure<AccountLockoutOptions>(configuration.GetSection(AccountLockoutOptions.SectionName));

        // Lockout policy
        services.AddScoped<ILockoutPolicy, LockoutPolicy>();

        return services;
    }
}
