using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,IConfiguration configuration)
    {
        var host = configuration["Database:Host"];
        var port = configuration["Database:Port"];
        var databaseName = configuration["Database:Name"];
        var username = configuration["Database:UserName"];
        var password = configuration["Database:Password"];

        if (string.IsNullOrWhiteSpace(host)) throw new InvalidOperationException("Database host is missing.");
        if (string.IsNullOrWhiteSpace(port)) throw new InvalidOperationException("Database port is missing.");
        if (string.IsNullOrWhiteSpace(databaseName)) throw new InvalidOperationException("Database name is missing.");
        if (string.IsNullOrWhiteSpace(username)) throw new InvalidOperationException("Database username is missing.");

        var connectionString = $"Host={host};" + $"Port={port};" + $"Database={databaseName};" + $"Username={username};" + $"Password={password};";

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
        return services;
    }
}