using Mailer.Core.Model;
using Mailer.Core.Services;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Sockets;

namespace Mailer.MailKit;

public class SmtpEmailTransport : BaseEmailTransport, IDisposable
{
    protected override SmtpConnectionOptions Options { get; }
    protected SmtpClient SmtpClient { get; }

    public SmtpEmailTransport(IOptions<SmtpConnectionOptions> options)
    {
        Options = options.Value;
        SmtpClient = new SmtpClient();
    }

    public void Dispose()
    {
        SmtpClient.Dispose();
        GC.SuppressFinalize(this);
    }

    protected override bool IsConnected => SmtpClient.IsConnected;

    protected override async Task ConnectAsync(CancellationToken cancelToken = default)
    {
        Logger?.LogDebug("Connecting to SMTP server.");
        try
        {
            await SmtpClient.ConnectAsync(Options.Host, Options.Port, cancellationToken: cancelToken);
            if (Options.Username != null)
            {
                Logger?.LogDebug("Authenticating with SMTP server.");
                await SmtpClient.AuthenticateAsync(Options.Username, Options.Password, cancelToken);
            }
        }
        catch (Exception ex) when (ex is SocketException or IOException or ProtocolException)
        {
            throw new TransportConnectionException("Failed to connect to SMTP server.", ex);
        }
        catch (AuthenticationException ex)
        {
            throw new TransportConnectionException("Failed to authenticate with SMTP server.", ex);
        }
    }

    protected override Task DisconnectAsync(CancellationToken cancelToken = default)
    {
        Logger?.LogDebug("Disconnecting from SMTP server.");
        return SmtpClient.DisconnectAsync(true, cancelToken);
    }

    protected override async Task SendEmailAsync(EmailMessage message, CancellationToken cancelToken = default)
    {
        try
        {
            await SmtpClient.SendAsync(message.ToMimeMessage(), cancelToken);
        }
        catch (ServiceNotConnectedException ex)
        {
            throw new TransportConnectionException(
                "SMTP client was not connected when sending message. Connection may have been lost.",
                ex);
        }
        catch (ServiceNotAuthenticatedException ex)
        {
            throw new TransportConnectionException(
                "SMTP server requires authentication but none was provided.",
                ex, isFatal: true);
        }
        catch (Exception ex) when (ex is InvalidOperationException or ParseException)
        {
            throw new TransportConnectionException(
                "Email message is invalid and cannot be sent.",
                ex, isFatal: true);
        }
        catch (Exception ex) when (ex is IOException or ProtocolException or CommandException)
        {
            throw new TransportConnectionException(
                "An exception occurred while sending an email.",
                ex);
        }
    }
}