using Mailer.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Model;
public class NewEmailMessageTests
{
    [Fact]
    public void ToEmailMessage_Result_Id_Is0()
    {
        var message = new NewEmailMessage()
            .ToEmailMessage();

        Assert.Equal(0, message.Id);
    }

    [Fact]
    public void ToEmailMessage_Result_WasSent_IsNull()
    {
        var message = new NewEmailMessage()
            .ToEmailMessage();

        Assert.Null(message.WasSent);
    }
}
