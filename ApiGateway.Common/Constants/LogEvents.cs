using Microsoft.Extensions.Logging;

namespace ApiGateway.Common.Constants
{
    public class LogEvents
    {
        public static readonly EventId LoginSuccess = new EventId(2001, "Login successful");
    }
}