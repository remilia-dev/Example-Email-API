using System.Collections.Generic;

using Mailer.Core.Model;
using MimeKit;
using Xunit;

namespace Tests.Model
{
    public class MessageTests
    {
        [Fact]
        public void ToMimeMessage_CorrectlyConvertsValidMessage()
        {
            const string TO_RECIPIENT = "to@example.org";
            const string CC_RECIPIENT = "carbon_copy@example.org";
            const string BCC_RECIPIENT = "blind_carbon_copy@example.org";
            var validMessage = new Message()
            {
                Sender = "sender@example.org",
                Subject = "Subject Line",
                HtmlBody = "<b>HTML Body</b>",
                Recipients = new List<Recipient>()
                {
                    new Recipient(TO_RECIPIENT),
                    new Recipient(CC_RECIPIENT, RecipientType.Cc),
                    new Recipient(BCC_RECIPIENT, RecipientType.Bcc),
                },
            };

            var mimeMessage = validMessage.ToMimeMessage();

            Assert.Equal(validMessage.Sender, mimeMessage.Sender.Address);
            Assert.Equal(validMessage.Subject, mimeMessage.Subject);
            Assert.Equal(validMessage.HtmlBody, mimeMessage.HtmlBody);
            Assert.Equal(TO_RECIPIENT,
                Assert.Single(mimeMessage.To).ToString());
            Assert.Equal(CC_RECIPIENT,
                Assert.Single(mimeMessage.Cc).ToString());
            Assert.Equal(BCC_RECIPIENT,
                Assert.Single(mimeMessage.Bcc).ToString());
        }

        [Fact]
        public void ToMimeMessage_ThrowsOnInvalidSender()
        {
            var invalidMessage = new Message()
            {
                Sender = "not a valid email address",
            };

            Assert.Throws<ParseException>(()
                => invalidMessage.ToMimeMessage());
        }

        [Fact]
        public void ToMimeMessage_ThrowsOnInvalidRecipient()
        {
            var invalidMessage = new Message()
            {
                Sender = "valid@example.org",
                Recipients = new List<Recipient>()
                {
                    new Recipient("definitely not a valid email")
                },
            };

            Assert.Throws<ParseException>(()
                => invalidMessage.ToMimeMessage());
        }
    }
}
