using Mailer.Core.Model;

namespace Mailer.Core.Services;

public interface IEmailTransport
{
    /// <summary>
    /// Asynchronously send an email message.
    /// </summary>
    /// <param name="message">The message to send. Must be a valid message.</param>
    /// <param name="cancelToken">Token to cancel the operation.</param>
    /// <exception cref="TransportConnectionException">
    /// Thrown when the connection to send the email failed for non-authentication reasons.
    /// Since this may be due to external variables (like Internet connectivity), trying
    /// again may result in the email being sent.
    /// </exception>
    Task SendEmailAsync(Message message, CancellationToken cancelToken = default);
}
