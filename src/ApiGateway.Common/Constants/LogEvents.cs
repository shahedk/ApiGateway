using Microsoft.Extensions.Logging;

namespace ApiGateway.Common.Constants
{
    public class LogEvents
    {
        public static readonly EventId LoginSuccess = new EventId(1001, "Login successful");
        public static readonly EventId NewKeyCreated = new EventId(1002, "Key created");
        public static readonly EventId NewKeyUpdated = new EventId(1003, "Key updated");
    }
}