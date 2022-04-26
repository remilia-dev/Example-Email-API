using Mailer.Core.Data;
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
            var db = scope.ServiceProvider.GetRequiredService<EmailDbContext>();
            var transport = scope.ServiceProvider.GetRequiredService<IEmailTransport>();
            transport.OnEmailResult += (email, successful, cancelToken) =>
            {
                email.WasSent = successful;
                db.Messages.Attach(email)
                    .Property(em => em.WasSent)
                    .IsModified = true;
                return db.SaveChangesAsync(cancelToken);
            };

            await transport.SendQueuedEmailsAsync(_emailQueue, stoppingToken);
        }
    }
}
