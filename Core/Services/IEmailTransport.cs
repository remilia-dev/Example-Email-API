namespace Mailer.Core.Services;

public interface IEmailTransport
{
    Task SendQueuedEmailsAsync(IEmailQueue emailQueue, CancellationToken cancelToken = default);
}
