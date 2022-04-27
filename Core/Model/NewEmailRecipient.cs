using System.ComponentModel.DataAnnotations;

namespace Mailer.Core.Model;

public class NewEmailRecipient
{
    [EmailAddress]
    public string Address { get; set; }
    [Range(typeof(RecipientType), "Min", "Max")]
    public RecipientType Type { get; set; }

    public NewEmailRecipient(string address, RecipientType type = default)
    {
        Address = address;
        Type = type;
    }

    public EmailRecipient ToEmailRecipient()
    {
        return new EmailRecipient(Address, Type);
    }
}