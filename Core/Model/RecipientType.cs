using System.Text.Json.Serialization;

namespace Mailer.Core.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RecipientType
{
    To = 0,
    Cc = 1,
    Bcc = 2,
    Min = To,
    Max = Bcc,
}
