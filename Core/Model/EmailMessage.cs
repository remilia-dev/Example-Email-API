using Microsoft.EntityFrameworkCore;

namespace Mailer.Core.Model;

[Index(nameof(Sender))]
[Index(nameof(CreatedOn))]
public class EmailMessage
{
    public int Id { get; set; }
    public string Sender { get; set; } = "";
    public string Subject { get; set; } = "";
    public string HtmlBody { get; set; } = "";
    public bool? WasSent { get; set; }
    public DateTime CreatedOn { get; internal set; }
    public DateTime LastModifiedOn { get; internal set; }

    public virtual ICollection<EmailRecipient> Recipients { get; set; } = new List<EmailRecipient>();
}
