using System.Runtime.Serialization;

namespace Mailer.Core.Services;

[Serializable]
public class EmailTransportException : Exception
{
    public bool IsFatal { get; set; }

    public EmailTransportException(bool isFatal = false)
        : this(null, isFatal) { }

    public EmailTransportException(string? message, bool isFatal = false)
        : this(message, null, isFatal) { }

    public EmailTransportException(string? message, Exception? innerException, bool isFatal = false)
        : base(message, innerException)
    {
        IsFatal = isFatal;
    }

    protected EmailTransportException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        IsFatal = info.GetBoolean(nameof(IsFatal));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(IsFatal), IsFatal);
    }
}
