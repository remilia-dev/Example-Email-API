namespace BaseAPI.Model
{
    public class Recipient
    {
        public int Id { get; private set; }
        public Message? Message { get; private set; }
        public string Address { get; set; }
        public RecipientType Type { get; set; }

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
