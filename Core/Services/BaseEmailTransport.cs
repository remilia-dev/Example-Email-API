using Mailer.Core.Model;
using Microsoft.Extensions.Logging;

namespace Mailer.Core.Services;

public abstract class BaseEmailTransport : IEmailTransport
{
    public event IEmailTransport.EmailResultHandler? OnEmailResult;
    protected ILogger? Logger { get; set; }
    protected abstract BaseEmailTransportOptions Options { get; }
    protected abstract bool IsConnected { get; }

    public async Task SendQueuedEmailsAsync(IEmailQueue emailQueue, CancellationToken cancelToken = default)
    {
        var message = emailQueue.TryDequeue();
        while (message is not null)
        {
            bool successful = await TrySendEmailAsync(message, cancelToken);
            if (OnEmailResult is not null)
            {
                await OnEmailResult.Invoke(message, successful);
            }
            message = emailQueue.TryDequeue();
        }
    }

    protected abstract Task ConnectAsync(CancellationToken cancelToken);
    protected abstract Task DisconnectAsync(CancellationToken cancelToken);
    protected abstract Task SendEmailAsync(EmailMessage message, CancellationToken cancelToken);

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
            } catch (EmailTransportException ex)
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

