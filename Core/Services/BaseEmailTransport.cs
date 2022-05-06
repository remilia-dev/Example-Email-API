using Mailer.Core.Model;
using Microsoft.Extensions.Logging;

namespace Mailer.Core.Services;

public abstract class BaseEmailTransport : IEmailTransport
{
    public event IEmailTransport.EmailResultHandler? OnEmailResult;
    /// <summary>
    /// A logger to log failed attempts at sending emails.
    /// </summary>
    protected abstract ILogger? Logger { get; }
    /// <summary>
    /// Options that customize the behavior of email transportation.
    /// </summary>
    /// <seealso cref="BaseEmailTransportOptions"/>
    protected abstract BaseEmailTransportOptions Options { get; }
    /// <summary>
    /// Whether the email transport is currently connected or not.
    /// </summary>
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
        await DisconnectAsync(cancelToken);
    }
    /// <summary>
    /// Connects the underlying email transport.
    /// </summary>
    /// <param name="cancelToken">A <see cref="CancellationToken"/> used to cancel the connection operation.</param>
    /// <exception cref="EmailTransportException">Thrown if the connection failed.</exception>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous connection operation.
    /// </returns>
    protected abstract Task ConnectAsync(CancellationToken cancelToken);
    /// <summary>
    /// Disconnects from the underlying email transport.
    /// </summary>
    /// <remarks>
    /// This method should not throw an exception if the server is not connected.
    /// </remarks>
    /// <param name="cancelToken">A <see cref="CancellationToken"/> used to cancel the disconnect operation.</param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous disconnect operation.
    /// </returns>
    protected abstract Task DisconnectAsync(CancellationToken cancelToken);
    /// <summary>
    /// Send an email message.
    /// </summary>
    /// <param name="message">The <see cref="EmailMessage"/> to send.</param>
    /// <param name="cancelToken">A <see cref="CancellationToken"/> used to cancel the sending operation.</param>
    /// <exception cref="EmailTransportException">Thrown if the email failed to send.</exception>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous sending operation.
    /// </returns>
    protected abstract Task SendEmailAsync(EmailMessage message, CancellationToken cancelToken);
    /// <summary>
    /// Attempts to send an <see cref="EmailMessage"/>; potentially making multiple attempts
    /// depending on the current <see cref="Options"/>.
    /// </summary>
    /// <param name="message">The <see cref="EmailMessage"/> to attempt to send.</param>
    /// <param name="cancelToken">A <see cref="CancellationToken"/> to cancel the sending operation.</param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous sending operation.
    /// The task result contains whether the email was successfully sent or not.
    /// </returns>
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
                await DisconnectAsync(cancelToken);
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

