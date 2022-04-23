namespace BaseAPI.Model
{
    public class Recipient
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Address { get; set; }
        public RecipientType Type { get; set; }

        public virtual Message? Message { get; set; }

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
