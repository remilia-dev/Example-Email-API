using Mailer.Core.Model;
using System;
using Xunit;

namespace Mailer.Tests.Model;

public class EmailRecipientTests
{
    [Fact]
    public void Constructor_Type_DefaultsToTo()
    {
        var recipient = new EmailRecipient("");

        Assert.Equal(RecipientType.To, recipient.Type);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenAddressIsNull()
    {
        Assert.Throws<ArgumentNullException>(()
            => new EmailRecipient(null!));
    }
}
