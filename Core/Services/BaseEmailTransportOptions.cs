namespace Mailer.Core.Services;

public class BaseEmailTransportOptions
{
    /// <summary>
    /// If sending an email fails the first time, this is the maximum number
    /// of times the transporter will attempt to send again.
    /// </summary>
    public int MaxRetries { get; set; } = 3;
    /// <summary>
    /// If sending an email fails the first time, this is the number of
    /// milliseconds before trying again.
    /// </summary>
    public int MillisecondDelayBetweenRetries { get; set; } = 0;
    /// <summary>
    /// Whether sensitive data, like email contents, are logged.
    /// </summary>
    public bool LogSensitiveData { get; set; } = false;
}

