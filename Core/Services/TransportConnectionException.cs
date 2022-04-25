using System.Runtime.Serialization;

namespace Mailer.Core.Services;

[Serializable]
public class TransportConnectionException : Exception
{
    public TransportConnectionException() { }

    public TransportConnectionException(string? message)
        : base(message) { }

    public TransportConnectionException(string? message, Exception? innerException)
        : base(message, innerException) { }

    protected TransportConnectionException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
