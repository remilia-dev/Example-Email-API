using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace Mailer.Core.Model
{
    [Index(nameof(Sender))]
    [Index(nameof(CreatedOn))]
    public class Message
    {
        public int Id { get; set; }
        public string Sender { get; set; } = "";
        public string Subject { get; set; } = "";
        public string HtmlBody { get; set; } = "";
        public bool? WasSent { get; set; }
        public DateTime CreatedOn { get; internal set; }
        public DateTime LastModifiedOn { get; internal set; }

        public virtual ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();

        /// <summary>
        /// Converts this message into a MimeMessage.
        /// </summary>
        /// <exception cref="ParseException">
        /// Thrown if <see cref="Sender"/> is an invalid email address or if
        /// <see cref="Recipients">any recipient</see> has an invalid email address.
        /// </exception>
        public MimeMessage ToMimeMessage()
        {
            var bodyBuilder = new BodyBuilder()
            {
                HtmlBody = this.HtmlBody,
            };

            var mimeMessage = new MimeMessage()
            {
                Subject = this.Subject,
                Body = bodyBuilder.ToMessageBody(),
                Sender = MailboxAddress.Parse(Sender),
            };

            foreach (var recipient in Recipients)
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
}
