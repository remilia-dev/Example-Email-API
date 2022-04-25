using Microsoft.EntityFrameworkCore;

namespace Mailer.Core.Model;

[Index(nameof(Address))]
public class EmailRecipient
{
    public int Id { get; set; }
    public int MessageId { get; set; }
    public string Address { get; set; }
    public RecipientType Type { get; set; }

    public virtual EmailMessage? Message { get; set; }

    public EmailRecipient(string address, RecipientType type = RecipientType.To)
    {
        Address = address;
        Type = type;
    }
}

public enum RecipientType
{
    To = 0,
    Cc = 1,
    Bcc = 2,
}
