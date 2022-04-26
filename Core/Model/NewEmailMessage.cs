using System.ComponentModel.DataAnnotations;

namespace Mailer.Core.Model;
public class NewEmailMessage : IValidatableObject
{
    [Required]
    [EmailAddress]
    public string? Sender { get; set; }
    [Required]
    public string? Subject { get; set; }
    [Required]
    public string? HtmlBody { get; set; }
    [Required]
    [MinLength(1)]
    public virtual ICollection<EmailRecipient>? Recipients { get; set; }

    public EmailMessage ToEmailMessage()
    {
        return new EmailMessage()
        {
            Sender = Sender,
            Subject = Subject,
            HtmlBody = HtmlBody,
            Recipients = Recipients,
        };
    }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Recipients == null)
        {
            yield break;
        }

        foreach (var recipient in Recipients)
        {
            if (recipient.Id != 0)
            {
                yield return new ValidationResult("Id cannot be specified in any Recipient.");
            }
            if (recipient.MessageId != 0)
            {
                yield return new ValidationResult("MessageId cannot be specified in any Recipient.");
            }
            if (recipient.Message is not null)
            {
                yield return new ValidationResult("Message cannot be specified in any Recipient.");
            }
        }
    }
}
