﻿using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;
using MimeKit;

namespace BaseAPI.Model
{
    public class Message : Identifiable<int>
    {
        [Attr]
        public string Sender { get; set; } = "";
        [Attr]
        public string Subject { get; set; } = "";
        [Attr]
        public string HtmlBody { get; set; } = "";
        [Attr]
        public bool? WasSent { get; set; }
        [Attr]
        public DateTime CreatedOn { get; internal set; }
        [Attr]
        public DateTime LastModifiedOn { get; internal set; }

        [HasMany]
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
