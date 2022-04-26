using Mailer.Core.Services;

namespace Mailer.MailKit;

public class SmtpConnectionOptions : BaseEmailTransportOptions
{
    public string Host { get; set; } = "";
    public int Port { get; set; } = 0;
    public string? Username { get; set; }
    public string? Password { get; set; }
}
