using Mailer.Core.Model;
using Microsoft.Extensions.Logging;

namespace Mailer.Core.Services;

public abstract class BaseEmailTransport : IEmailTransport
{
    protected ILogger? Logger { get; set; }
    protected abstract BaseEmailTransportOptions Options { get; }

    protected abstract bool IsConnected { get; }
    protected abstract Task ConnectAsync(CancellationToken cancelToken);
    protected abstract Task DisconnectAsync(CancellationToken cancelToken);
    protected abstract Task SendEmailAsync(EmailMessage message, CancellationToken cancelToken);

    public async Task SendQueuedEmailsAsync(IEmailQueue emailQueue, CancellationToken cancelToken = default)
    {
        var message = emailQueue.TryDequeue();
        while (message is not null)
        {
            await TrySendEmailAsync(message, cancelToken);
            message = emailQueue.TryDequeue();
        }
    }

    protected async Task<bool> TrySendEmailAsync(EmailMessage message, CancellationToken cancelToken = default)
    {
        int attempts = 0;
        do
        {
            try
            {
                if (!IsConnected)
                {
                    await ConnectAsync(cancelToken);
                }
                await SendEmailAsync(message, cancelToken);
                return true;
            } catch (TransportConnectionException ex)
            {
                Logger?.LogError(ex, "Failed to send message.");
                // Disconnect just in case if the connection is bad.
                if (IsConnected)
                {
                    await DisconnectAsync(cancelToken);
                }
                // There is no point in retrying if a fatal exception occurred.
                if (ex.IsFatal)
                {
                    break;
                }
            }
        } while (attempts++ < Options.MaxRetries);

        if (Options.LogSensitiveData)
        {
            Logger?.LogWarning("Failed to send email message: {@message}", message);
        } else
        {
            Logger?.LogWarning("Failed to send email message");
        }
        return false;
    }
}

