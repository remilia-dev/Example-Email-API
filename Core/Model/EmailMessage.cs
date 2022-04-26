using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Mailer.Core.Model;

[Index(nameof(Sender))]
[Index(nameof(CreatedOn))]
public class EmailMessage : NewEmailMessage
{
    public int Id { get; set; }
    public bool? WasSent { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastModifiedOn { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        // NewEmailMessage enforces that no Recipient has an Id, MessageId,
        // or Message to ensure the data is "new".
        //
        // EmailMessage shouldn't have this constraint since it may be a
        // message pulled from the database.
        yield break;
    }
}
