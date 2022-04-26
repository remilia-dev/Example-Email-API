using Mailer.Core.Model;
using Xunit;

namespace Mailer.Tests.Model;

public class EmailRecipientTests
{
    [Fact]
    public void Recipient_Type_DefaultsToTo()
    {
        Assert.Equal(RecipientType.To,
            new EmailRecipient("valid@example.org").Type);
    }
}
