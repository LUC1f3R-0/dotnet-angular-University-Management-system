using Infrastructure.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace Infrastructure.Email;

public class SmtpConnectionValidator
{
    private readonly SmtpOptions _options;

    public SmtpConnectionValidator(IOptions<SmtpOptions> options)
    {
        _options = options.Value;
    }

    public async Task ValidateAsync()
    {
        using var client = new SmtpClient();

        await client.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.Auto);
        await client.AuthenticateAsync(_options.Username, _options.Password);
        await client.DisconnectAsync(true);
    }
}
