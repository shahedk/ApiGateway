using System.Net;

namespace ApiGateway.Common.Exceptions
{
    public class DataValidationException: ApiGatewayException
    {
        public DataValidationException(string message, HttpStatusCode errorCode) : base(message,errorCode)
        {
        }
    }
}