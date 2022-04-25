using Mailer.Core.Model;
using MimeKit;

namespace Mailer.MailKit;

public static class EmailMessageExtensions
{
    /// <summary>
    /// Converts this message into a MimeMessage.
    /// </summary>
    /// <exception cref="ParseException">
    /// Thrown if <see cref="Sender"/> is an invalid email address or if
    /// <see cref="Recipients">any recipient</see> has an invalid email address.
    /// </exception>
    public static MimeMessage ToMimeMessage(this EmailMessage m)
    {
        var bodyBuilder = new BodyBuilder()
        {
            HtmlBody = m.HtmlBody,
        };

        var mimeMessage = new MimeMessage()
        {
            Subject = m.Subject,
            Body = bodyBuilder.ToMessageBody(),
            Sender = MailboxAddress.Parse(m.Sender),
        };

        foreach (var recipient in m.Recipients)
        {
            var addressList = recipient.Type switch
            {
                RecipientType.To => mimeMessage.To,
                RecipientType.Cc => mimeMessage.Cc,
                RecipientType.Bcc => mimeMessage.Bcc,
                _ => throw new NotImplementedException($"Unknown recipient type: {recipient.Type}"),
            };
            addressList.Add(MailboxAddress.Parse(recipient.Address));
        }

        return mimeMessage;
    }
}
