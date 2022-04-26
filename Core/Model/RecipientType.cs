namespace Mailer.Core.Model;

public enum RecipientType
{
    To = 0,
    Cc = 1,
    Bcc = 2,
    Min = To,
    Max = Bcc,
}
