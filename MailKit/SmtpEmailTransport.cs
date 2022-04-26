using Mailer.Core.Model;
using Mailer.Core.Services;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Sockets;

namespace Mailer.MailKit;

public class SmtpEmailTransport : IEmailTransport, IDisposable
{
    private readonly SmtpConnectionOptions _options;
    private readonly SmtpClient _smtp;

    public SmtpEmailTransport(IOptions<SmtpConnectionOptions> options)
    {
        _options = options.Value;
        _smtp = new SmtpClient();
    }

    public void Dispose()
    {
        _smtp.Dispose();
        GC.SuppressFinalize(this);
    }

    public bool IsConnected => _smtp.IsConnected;

    public async Task ConnectAsync(CancellationToken cancelToken = default)
    {
        try
        {
            await _smtp.ConnectAsync(_options.Host, _options.Port, cancellationToken: cancelToken);
            if (_options.Username != null)
            {
                await _smtp.AuthenticateAsync(_options.Username, _options.Password, cancelToken);
            }
        } catch (Exception ex) when (ex is SocketException or IOException or ProtocolException)
        {
            throw new TransportConnectionException("Failed to connect to SMTP server.", ex);
        } catch (AuthenticationException ex)
        {
            throw new TransportConnectionException("Failed to authenticate with SMTP server.", ex);
        }
    }

    public Task DisconnectAsync(CancellationToken cancelToken = default)
        => _smtp.DisconnectAsync(true, cancelToken);

    public async Task SendEmailAsync(EmailMessage message, CancellationToken cancelToken = default)
    {
        try
        {
            await _smtp.SendAsync(message.ToMimeMessage(), cancelToken);
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
                ex);
        }
        catch (Exception ex) when (ex is InvalidOperationException or ParseException)
        {
            throw new TransportConnectionException(
                "Email message is invalid and cannot be sent.",
                ex);
        }
        catch (Exception ex) when (ex is IOException or ProtocolException or CommandException)
        {
            throw new TransportConnectionException(
                "An exception occurred while sending an email.",
                ex);
        }
    }
}