using Mailer.Core.Model;

namespace Mailer.Core.Services;

public interface IEmailTransport
{
    bool IsConnected { get; }
    Task ConnectAsync(CancellationToken cancelToken = default);
    Task DisconnectAsync(CancellationToken cancelToken = default);
    Task SendEmailAsync(EmailMessage message, CancellationToken cancelToken = default);
}
