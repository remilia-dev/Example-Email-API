using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mailer.Core.Services;

public class BackgroundEmailService : BackgroundService
{
    private readonly IEmailQueue _emailQueue;
    private readonly IServiceProvider _serviceProvider;

    public BackgroundEmailService(IEmailQueue emailQueue, IServiceProvider serviceProvider)
    {
        _emailQueue = emailQueue;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _emailQueue.WaitForEmail(stoppingToken);

            using var scope = _serviceProvider.CreateAsyncScope();
            var transport = scope.ServiceProvider.GetRequiredService<IEmailTransport>();
            await transport.SendQueuedEmailsAsync(_emailQueue, stoppingToken);
        }
    }
}
