
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Email;

public class SmtpConnectionValidator
{
    private readonly IConfiguration _configuration;

    public SmtpConnectionValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ValidateAsync()
    {
        var host = _configuration["Smtp:Host"] ?? throw new InvalidOperationException("SMTP host is missing.");
        var username = _configuration["Smtp:Username"] ?? throw new InvalidOperationException("SMTP username is missing.");
        var password = _configuration["Smtp:Password"] ?? throw new InvalidOperationException("SMTP password is missing.");
        var port = _configuration.GetValue<int>("Smtp:Port");
        
        using var client = new SmtpClient();

        await client.ConnectAsync(host, port, SecureSocketOptions.Auto);
        await client.AuthenticateAsync(username, password);
        await client.DisconnectAsync(true);
    }
}