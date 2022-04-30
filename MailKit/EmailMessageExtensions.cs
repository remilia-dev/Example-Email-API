using Mailer.Core.Model;
using MimeKit;

namespace Mailer.MailKit;

public static class EmailMessageExtensions
{
    /// <summary>
    /// Converts an <see cref="EmailMessage"/> message into a <see cref="MimeMessage"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// `m` is `null`
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <para><see cref="EmailMessage.Sender"/> is null</para>
    /// <para>- or -</para>
    /// <para><see cref="EmailMessage.Subject"/> is null</para>
    /// <para>- or -</para>
    /// <para><see cref="EmailMessage.HtmlBody"/> is null</para>
    /// <para>- or -</para>
    /// <para><see cref="EmailMessage.Recipients"/> is null</para>
    /// </exception>
    /// <exception cref="ParseException">
    /// <para><see cref="EmailMessage.Sender"/> is an invalid email address</para>
    /// <para>- or -</para>
    /// <para><see cref="EmailMessage.Recipients">any recipient</see> has an invalid email address</para>
    /// </exception>
    public static MimeMessage ToMimeMessage(this EmailMessage m)
    {
        if (m is null)
        {
            throw new ArgumentNullException(nameof(m));
        } else if (m.Sender is null)
        {
            throw new InvalidOperationException(
                $"Cannot convert an EmailMessage with a null {nameof(m.Sender)} property to a MimeMessage.");
        } else if (m.Subject is null)
        {
            throw new InvalidOperationException(
                $"Cannot convert an EmailMessage with a null {nameof(m.Subject)} property to a MimeMessage.");
        } else if (m.HtmlBody is null)
        {
            throw new InvalidOperationException(
                $"Cannot convert an EmailMessage with a null {nameof(m.HtmlBody)} property to a MimeMessage.");
        } else if (m.Recipients is null)
        {
            throw new InvalidOperationException(
                $"Cannot convert an EmailMessage with a null {nameof(m.Recipients)} property to a MimeMessage.");
        }

        var mimeMessage = new MimeMessage()
        {
            Subject = m.Subject,
            Body = new TextPart("html", m.HtmlBody),
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
