using Mailer.Core.Model;
using Xunit;

namespace Mailer.Tests.Model;

public class RecipientTests
{
    [Fact]
    public void Recipient_Type_DefaultsToTo()
    {
        Assert.Equal(RecipientType.To,
            new Recipient("valid@example.org").Type);
    }

    [Fact]
    public void RecipientType_IntValuesRemainTheSame()
    {
        // It's important to ensure that RecipientType enum values keep the same int value as
        // changing them would also change how saved values are interpreted.
        Assert.Equal(0, (int)RecipientType.To);
        Assert.Equal(1, (int)RecipientType.Cc);
        Assert.Equal(2, (int)RecipientType.Bcc);
    }
}
