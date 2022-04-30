using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Mailer.Core.Model;

[Owned]
[Index(nameof(Address))]
public class EmailRecipient
{
    public string Address { get; set; }
    [Range(typeof(RecipientType), "Min", "Max")]
    public RecipientType Type { get; set; }

    public EmailRecipient(string address, RecipientType type = RecipientType.To)
    {
        Address = address;
        Type = type;
    }
}
