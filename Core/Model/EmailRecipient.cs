using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Mailer.Core.Model;

[Index(nameof(Address))]
public class EmailRecipient
{
    public int Id { get; set; }
    public int MessageId { get; set; }
    [EmailAddress]
    public string Address { get; set; }
    [Range(typeof(RecipientType), "Min", "Max")]
    public RecipientType Type { get; set; }

    public virtual EmailMessage? Message { get; set; }

    public EmailRecipient(string address, RecipientType type = RecipientType.To)
    {
        Address = address;
        Type = type;
    }
}
