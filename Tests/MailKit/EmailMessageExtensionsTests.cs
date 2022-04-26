﻿using System.Collections.Generic;

using Mailer.Core.Model;
using Mailer.MailKit;
using MimeKit;
using Xunit;

namespace Mailer.Tests.MailKit;

public class EmailMessageExtensionsTests
{
    [Fact]
    public void ToMimeMessage_CorrectlyConvertsValidMessage()
    {
        const string TO_RECIPIENT = "to@example.org";
        const string CC_RECIPIENT = "carbon_copy@example.org";
        const string BCC_RECIPIENT = "blind_carbon_copy@example.org";
        var validMessage = new EmailMessage()
        {
            Sender = "sender@example.org",
            Subject = "Subject Line",
            HtmlBody = "<b>HTML Body</b>",
            Recipients = new List<EmailRecipient>()
            {
                new EmailRecipient(TO_RECIPIENT),
                new EmailRecipient(CC_RECIPIENT, RecipientType.Cc),
                new EmailRecipient(BCC_RECIPIENT, RecipientType.Bcc),
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
        var invalidMessage = new EmailMessage()
        {
            Sender = "not a valid email address",
        };

        Assert.Throws<ParseException>(()
            => invalidMessage.ToMimeMessage());
    }

    [Fact]
    public void ToMimeMessage_ThrowsOnInvalidRecipient()
    {
        var invalidMessage = new EmailMessage()
        {
            Sender = "valid@example.org",
            Recipients = new List<EmailRecipient>()
            {
                new EmailRecipient("definitely not a valid email")
            },
        };

        Assert.Throws<ParseException>(()
            => invalidMessage.ToMimeMessage());
    }
}