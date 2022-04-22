using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace BaseAPI.Model
{
    public class Recipient : Identifiable<int>
    {
        public int MessageId { get; private set; }
        [Attr]
        public string Address { get; set; }
        [Attr]
        public RecipientType Type { get; set; }

        [HasOne]
        public Message? Message { get; private set; }

        public Recipient(string address, RecipientType type = RecipientType.To)
        {
            Address = address;
            Type = type;
        }
    }

    public enum RecipientType
    {
        To = 0,
        Cc = 1,
        Bcc = 2,
    }
}
