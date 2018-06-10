using Microsoft.Extensions.Logging;

namespace ApiGateway.Common.Constants
{
    public class LogEvents
    {
        public class Error
        {
            public static readonly EventId RegenerateKeyFailed = new EventId(5001, "Regenerating Key Failed");
        }

        public class Warning
        {
            public static readonly EventId LoginForbidden = new EventId(4001, "Login forbidden");
            public static readonly EventId InvalidPublicKey = new EventId(4002, "Invalid public key");
        }

        public class Info
        {
            public static readonly EventId LoginSuccess = new EventId(2001, "Login successful");
        }

    }
}