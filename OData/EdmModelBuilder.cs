using Mailer.Core.Model;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Mailer.OData;

public static class EdmModelBuilder
{
    public static IEdmModel GetV1Model()
    {
        var builder = new ODataConventionModelBuilder();

        builder.EntitySet<EmailMessage>("Messages").EntityType
            .Count()
            .Expand(0, SelectExpandType.Automatic, "Recipients")
            .Filter("Sender", "WasSent", "CreatedOn")
            // Allows filtering message by recipient addresses and types
            .Filter("Recipients", "Address", "Type")
            .Select()
            .OrderBy("Sender", "CreatedOn")
            .Page(50, 25);

        builder.EntitySet<EmailRecipient>("Recipients").EntityType
            .Expand(0, "Message")
            .Filter("Address", "Type")
            .OrderBy("Address")
            .Page(50, 25);

        return builder.GetEdmModel();
    }
}
