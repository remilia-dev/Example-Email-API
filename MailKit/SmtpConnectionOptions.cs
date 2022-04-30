using Mailer.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace Mailer.MailKit;

public class SmtpConnectionOptions : BaseEmailTransportOptions, IValidatableObject
{
    [Required]
    public string? Host { get; set; } = "";
    [Range(0, ushort.MaxValue)]
    public int Port { get; set; } = 0;
    public string? Username { get; set; }
    public string? Password { get; set; }

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
