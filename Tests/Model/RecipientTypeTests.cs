
using Mailer.Core.Model;
using System;
using Xunit;

namespace Mailer.Tests.Model;
public class RecipientTypeTests
{
    [Theory]
    [InlineData(0, RecipientType.To)]
    [InlineData(1, RecipientType.Cc)]
    [InlineData(2, RecipientType.Bcc)]
    public void IntValueRemainsTheSame(int originalValue, RecipientType actual)
    {
        // It's important to ensure that RecipientType enum values keep the
        // same int value as changing them would also change how saved values
        // are interpreted.
        Assert.Equal(originalValue, (int)actual);
    }

    [Fact]
    public void DefaultValueIsTo()
    {
        RecipientType defaultType = default;
        Assert.Equal(RecipientType.To, defaultType);
    }

    [Fact]
    public void AllValues_AreInRange()
    {
        foreach (var recipientType in Enum.GetValues<RecipientType>())
        {
            Assert.InRange(recipientType, RecipientType.Min, RecipientType.Max);
        }
    }
}
