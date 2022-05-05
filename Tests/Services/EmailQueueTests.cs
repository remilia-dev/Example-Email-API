using Mailer.Core.Model;
using Mailer.Core.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Services;
public class EmailQueueTests
{
    [Fact]
    public async Task WaitForEmail_Throws_IfQueueEmptyAndOperationCancelled()
    {
        var cancelSource = new CancellationTokenSource();
        cancelSource.Cancel();
        var emailQueue = new EmailQueue();

        await Assert.ThrowsAsync<TaskCanceledException>(()
            => emailQueue.WaitForEmailAsync(cancelSource.Token));
    }

    [Fact]
    public void TryDequeue_ReturnsNull_IfNothingInQueue()
    {
        var emailQueue = new EmailQueue();

        Assert.Null(emailQueue.TryDequeue());
    }

    [Fact]
    public async Task TryDequeue_ReturnsFirstEmailInQueue()
    {
        var emailQueue = new EmailQueue();
        var message = BlankEmailMessage();

        await emailQueue.EnqueueAsync(message);

        Assert.Same(message, emailQueue.TryDequeue());
    }

    private static EmailMessage BlankEmailMessage()
    {
        return new EmailMessage()
        {
            Sender = "",
            Subject = "",
            HtmlBody = "",
            Recipients = new List<EmailRecipient>(),
        };
    }
}
