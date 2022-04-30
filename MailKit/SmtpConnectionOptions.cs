using Mailer.Core.Services;
using MailKit.Security;
using System.ComponentModel.DataAnnotations;

namespace Mailer.MailKit;

public class SmtpConnectionOptions : BaseEmailTransportOptions, IValidatableObject
{
    /// <summary>
    /// The host SMTP server to connect to.
    /// </summary>
    [Required]
    public string? Host { get; set; } = "";
    /// <summary>
    /// The port number to connect to. 0 will select the default port.
    /// </summary>
    /// <remarks>
    /// The default port is 465 when <see cref="SecureSocketOptions"/> is
    /// <see cref="SecureSocketOptions.SslOnConnect"/> and 25 otherwise.
    /// </remarks>
    [Range(0, ushort.MaxValue)]
    public int Port { get; set; } = 0;
    /// <summary>
    /// An optional username to use to authenticate with the SMTP server. If not
    /// <c>null</c>, then <see cref="Password"/> must not be <c>null</c> as well.
    /// </summary>
    public string? Username { get; set; }
    /// <summary>
    /// An optional password to use to authenticate with the SMTP server. If not
    /// <c>null</c>, then <see cref="Username"/> must not be <c>null</c> as well.
    /// </summary>
    public string? Password { get; set; }
    /// <summary>
    /// Secure socket options when connecting to the SMTP server. See MailKit's
    /// documentation for more information.
    /// </summary>
    public SecureSocketOptions SecureSocketOptions { get; set; } = SecureSocketOptions.Auto;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Username is not null && Password is null)
        {
            yield return new ValidationResult("Password property must be filled since Username is filled.");
        } else if (Username is null && Password is not null)
        {
            yield return new ValidationResult("Username property must be filled since Password is filled.");
        }
        yield break;
    }
}
