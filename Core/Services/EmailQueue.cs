using Mailer.Core.Model;
using System.Threading.Channels;

namespace Mailer.Core.Services;

public class EmailQueue : IEmailQueue
{
    private readonly Channel<EmailMessage> _channel;
    /// <summary>
    /// Creates an unbounded email queue.
    /// </summary>
    public EmailQueue()
    {
        _channel = Channel.CreateUnbounded<EmailMessage>();
    }
    /// <summary>
    /// Creates a bounded email queue. Callers adding to the queue will have to
    /// wait if the queue reaches maximum capacity.
    /// </summary>
    /// <param name="bound">The maximum number of items the queue can hold.</param>
    public EmailQueue(int bound)
    {
        _channel = Channel.CreateBounded<EmailMessage>(bound);
    }

    public Task EnqueueAsync(EmailMessage message, CancellationToken cancelToken = default)
        => _channel.Writer.WriteAsync(message, cancelToken).AsTask();

    public Task WaitForEmail(CancellationToken cancelToken = default)
        => _channel.Reader.WaitToReadAsync(cancelToken).AsTask();

    public EmailMessage? TryDequeue()
    {
        _channel.Reader.TryRead(out var message);
        return message;
    }
}
