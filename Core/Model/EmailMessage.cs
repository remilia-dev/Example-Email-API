using Microsoft.EntityFrameworkCore;

namespace Mailer.Core.Model;

[Index(nameof(Sender))]
[Index(nameof(CreatedOn))]
public class EmailMessage : NewEmailMessage
{
    public int Id { get; set; }
    public bool? WasSent { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastModifiedOn { get; set; }
}
