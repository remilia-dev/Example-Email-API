using System.ComponentModel.DataAnnotations;

namespace Mailer.Core.Model;
public class NewEmailMessage
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
}
