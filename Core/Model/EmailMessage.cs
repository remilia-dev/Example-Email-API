using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mailer.Core.Model;

[Index(nameof(Sender))]
[Index(nameof(CreatedOn))]
public class EmailMessage
{
    public int Id { get; set; }
    [EmailAddress]
    public string Sender { get; set; } = "";
    public string Subject { get; set; } = "";
    public string HtmlBody { get; set; } = "";
    public bool? WasSent { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastModifiedOn { get; set; }

    public virtual ICollection<EmailRecipient> Recipients { get; set; } = new List<EmailRecipient>();
}
