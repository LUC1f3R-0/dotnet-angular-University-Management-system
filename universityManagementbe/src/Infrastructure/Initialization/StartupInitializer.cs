using Infrastructure.Email;
using Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Initialization;

public class StartupInitializer
{
    private readonly ApplicationDbContext _dbContext;
    private readonly SmtpConnectionValidator _smtpValidator;
    private readonly ILogger<StartupInitializer> _logger;

    public StartupInitializer(ApplicationDbContext dbContext,SmtpConnectionValidator smtpValidator,ILogger<StartupInitializer> logger)
    {
        _dbContext = dbContext;
        _smtpValidator = smtpValidator;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        _logger.LogInformation("Checking database connection...");
        var canConnect = await _dbContext.Database.CanConnectAsync();

        if (!canConnect)
        {
            _logger.LogCritical("Database connection failed.");
            throw new InvalidOperationException("Unable to connect to the database.");
        }
        
        _logger.LogInformation("Database connection successful.");
        _logger.LogInformation("Checking SMTP connection...");
        
        try
        {
            await _smtpValidator.ValidateAsync();
            _logger.LogInformation("SMTP connection successful.");
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception,"SMTP connection failed.");
        }
    }
}