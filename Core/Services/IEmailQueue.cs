using Mailer.Core.Model;

namespace Mailer.Core.Services;

public interface IEmailQueue
{
    /// <summary>
    /// Adds an email message to the queue.
    /// </summary>
    /// <param name="message">The <see cref="EmailMessage"/> to add to the queue.</param>
    /// <param name="cancelToken">A <see cref="CancellationToken"/> to cancel enqueue operation.</param>
    /// <returns>
    /// A <see cref="Task"/> that will complete when the email has been
    /// added to the queue.
    /// </returns>
    Task EnqueueAsync(EmailMessage message, CancellationToken cancelToken = default);
    /// <summary>
    /// Waits until an email message has entered the queue.
    /// </summary>
    /// <param name="cancelToken">A <see cref="CancellationToken"/> to cancel the wait operation.</param>
    /// <returns>
    /// A <see cref="Task"/> that will complete when an email is in the queue.
    /// </returns>
    Task WaitForEmail(CancellationToken cancelToken = default);
    /// <summary>
    /// Attempts to remove an email message from the queue.
    /// </summary>
    /// <returns>The email message or null if there was none in the queue.</returns>
    EmailMessage? TryDequeue();
}
