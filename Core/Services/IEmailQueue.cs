using Mailer.Core.Model;

namespace Mailer.Core.Services;

public interface IEmailQueue
{
    /// <summary>
    /// Adds an email message to the queue.
    /// </summary>
    /// <param name="message">The <see cref="EmailMessage"/> to add to the queue.</param>
    /// <param name="cancelToken">A <see cref="CancellationToken"/> used to cancel enqueue operation.</param>
    /// <returns>
    /// A <see cref="Task"/> that represents the enqueue operation.
    /// </returns>
    Task EnqueueAsync(EmailMessage message, CancellationToken cancelToken = default);
    /// <summary>
    /// Waits until an email message has entered the queue.
    /// </summary>
    /// <param name="cancelToken">A <see cref="CancellationToken"/> used to cancel the wait operation.</param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous wait operation.
    /// </returns>
    Task WaitForEmailAsync(CancellationToken cancelToken = default);
    /// <summary>
    /// Attempts to remove an email message from the queue.
    /// </summary>
    /// <returns>
    /// The <see cref="EmailMessage"/> removed from the queue or null if the
    /// queue is empty.
    /// </returns>
    EmailMessage? TryDequeue();
}
