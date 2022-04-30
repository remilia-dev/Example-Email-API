using System;
using System.Collections.Generic;

using Mailer.Core.Model;
using Mailer.MailKit;
using MimeKit;
using Xunit;

namespace Mailer.Tests.MailKit;

public class EmailMessageExtensionsTests
{
    [Fact]
    public void ToMimeMessage_CorrectlyConverts_Sender()
    {
        const string SENDER = "valid@example.org";
        var message = ValidEmailMessage();
        message.Sender = SENDER;

        var mimeMessage = message.ToMimeMessage();

        Assert.Equal(SENDER, mimeMessage.Sender.Address);
    }

    [Fact]
    public void ToMimeMessage_CorrectlyConverts_SubjectAndBody()
    {
        const string SUBJECT_LINE = "Subject Line Text";
        const string HTML_BODY = "<b>HTML</b> Body Text";
        var message = ValidEmailMessage();
        message.Subject = SUBJECT_LINE;
        message.HtmlBody = HTML_BODY;

        var mimeMessage = message.ToMimeMessage();

        Assert.Equal(SUBJECT_LINE, mimeMessage.Subject);
        Assert.Equal(HTML_BODY, mimeMessage.HtmlBody);
    }

    [Fact]
    public void ToMimeMessage_CorrectlyConverts_Recipients()
    {
        const string TO_RECIPIENT = "to@example.org";
        const string CC_RECIPIENT = "carbon_copy@example.org";
        const string BCC_RECIPIENT = "blind_carbon_copy@example.org";
        var message = ValidEmailMessage();
        message.Recipients = new List<EmailRecipient>()
        {
            new EmailRecipient(TO_RECIPIENT),
            new EmailRecipient(CC_RECIPIENT, RecipientType.Cc),
            new EmailRecipient(BCC_RECIPIENT, RecipientType.Bcc),
        };

        var mimeMessage = message.ToMimeMessage();

        Assert.Equal(TO_RECIPIENT,
            Assert.Single(mimeMessage.To).ToString());
        Assert.Equal(CC_RECIPIENT,
            Assert.Single(mimeMessage.Cc).ToString());
        Assert.Equal(BCC_RECIPIENT,
            Assert.Single(mimeMessage.Bcc).ToString());
    }

    [Fact]
    public void ToMimeMessage_ThrowsInvalidOperationException_OnNullSender()
    {
        var message = ValidEmailMessage();
        message.Sender = null;

        Assert.Throws<InvalidOperationException>(()
            => message.ToMimeMessage());
    }

    [Fact]
    public void ToMimeMessage_ThrowsInvalidOperationException_OnNullSubject()
    {
        var message = ValidEmailMessage();
        message.Subject = null;

        Assert.Throws<InvalidOperationException>(()
            => message.ToMimeMessage());
    }

    [Fact]
    public void ToMimeMessage_ThrowsInvalidOperationException_OnNullHtmlBody()
    {
        var message = ValidEmailMessage();
        message.HtmlBody = null;

        Assert.Throws<InvalidOperationException>(()
            => message.ToMimeMessage());
    }

    [Fact]
    public void ToMimeMessage_ThrowsInvalidOperationException_OnNullRecipients()
    {
        var message = ValidEmailMessage();
        message.Recipients = null;

        Assert.Throws<InvalidOperationException>(()
            => message.ToMimeMessage());
    }

    [Fact]
    public void ToMimeMessage_ThrowsParseException_OnInvalidSender()
    {
        var message = ValidEmailMessage();
        message.Sender = "not a valid email";

        Assert.Throws<ParseException>(()
            => message.ToMimeMessage());
    }

    [Fact]
    public void ToMimeMessage_ThrowsParseException_OnInvalidRecipientAddress()
    {
        var message = ValidEmailMessage();
        message.Recipients!.Add(new EmailRecipient("not a valid email"));

        Assert.Throws<ParseException>(()
            => message.ToMimeMessage());
    }

    private static EmailMessage ValidEmailMessage()
    {
        return new EmailMessage()
        {
            Sender = "valid@example.org",
            Subject = "",
            HtmlBody = "",
            Recipients = new List<EmailRecipient>(),
        };
    }
}
