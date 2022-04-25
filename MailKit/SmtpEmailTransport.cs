using Mailer.Core.Model;
using Mailer.Core.Services;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using System.Net.Sockets;

namespace Mailer.MailKit;

public class SmtpEmailTransport : IEmailTransport
{
    private readonly SmtpConnectionOptions _options;

    public SmtpEmailTransport(IOptions<SmtpConnectionOptions> options)
    {
        _options = options.Value;
    }

    public async Task SendEmailAsync(Message message, CancellationToken cancelToken = default)
    {
        try
        {
            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_options.Host, _options.Port, cancellationToken: cancelToken);
            if (_options.Username != null)
            {
                await smtpClient.AuthenticateAsync(_options.Username, _options.Password, cancelToken);
            }
            await smtpClient.SendAsync(message.ToMimeMessage(), cancelToken);
            await smtpClient.DisconnectAsync(true, cancelToken);
        }
        catch (Exception ex) when (ex is IOException or SocketException or ProtocolException or CommandException)
        {
            throw new TransportConnectionException("An exception occurred while sending an email.", ex);
        }
    }
}