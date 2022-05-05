using Mailer.Core.Model;

namespace Mailer.Core.Services;

public interface IEmailTransport
{
    /// <summary>
    /// An event handler for when an email has either successfully been sent or failed to send.
    /// </summary>
    /// <param name="email">The <see cref="EmailMessage"/> that the transport has a result for.</param>
    /// <param name="wasSent"><c>true</c> only if the email was successfully sent to the SMTP server.</param>
    /// <returns>A task that completes when the event handler completes.</returns>
    public delegate Task EmailResultHandler(EmailMessage email, bool wasSent);
    /// <summary>
    /// Occurs when an email has either been sent or has failed to send.
    /// </summary>
    event EmailResultHandler? OnEmailResult;
    /// <summary>
    /// Sends every <see cref="EmailMessage"/> in the
    /// <paramref name="emailQueue">queue</paramref>.
    /// </summary>
    /// <remarks>
    /// Emails can be added to the queue while this operation is executing.
    /// </remarks>
    /// <param name="emailQueue">The queue of emails to send.</param>
    /// <param name="cancelToken">
    /// A <see cref="CancellationToken"/> used to cancel the email sending operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous email sending operation.
    /// </returns>
    Task SendQueuedEmailsAsync(IEmailQueue emailQueue, CancellationToken cancelToken = default);
}
