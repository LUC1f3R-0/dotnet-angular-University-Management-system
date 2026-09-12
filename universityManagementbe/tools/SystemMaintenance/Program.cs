using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);


// --------------------------------------------------
// Configuration
// --------------------------------------------------
builder.Configuration.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"),optional: false,reloadOnChange: false).AddUserSecrets<Program>(optional: false);

// --------------------------------------------------
// Services
// --------------------------------------------------

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<RoleSeeder>();
builder.Services.AddScoped<InitialAdminSeeder>();
builder.Services.AddScoped<DatabaseSeeder>();


using var app = builder.Build();


// --------------------------------------------------
// Command
// --------------------------------------------------

if (args.Length == 0)
{
    Console.WriteLine("No command provided.");
    Console.WriteLine("Usage: init");
    return;
}

var command = args[0].ToLowerInvariant();

switch (command)
{
    case "init":
        await RunInitAsync(app.Services);
        break;

    default:
        Console.WriteLine($"Unknown command: {command}");
        break;
}


// --------------------------------------------------
// Init
// --------------------------------------------------

static async Task RunInitAsync(IServiceProvider services)
{
    Console.WriteLine("Initializing University Management System...");
    Console.WriteLine();

    Console.Write("Admin name: ");
    var name = Console.ReadLine();

    Console.Write("Admin email: ");
    var email = Console.ReadLine();

    Console.Write("Admin password: ");
    var password = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
    {
        Console.WriteLine("Name, email and password are required.");
        return;
    }

    using var scope = services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync(name, email, password);
    Console.WriteLine();
    Console.WriteLine("Initialization completed successfully.");
}