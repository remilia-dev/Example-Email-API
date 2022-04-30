﻿using Mailer.Core.Model;

namespace Mailer.Core.Services;

public interface IEmailTransport
{
    public delegate Task EmailResultHandler(EmailMessage email, bool successful);
    event EmailResultHandler? OnEmailResult;

    Task SendQueuedEmailsAsync(IEmailQueue emailQueue, CancellationToken cancelToken = default);
}
