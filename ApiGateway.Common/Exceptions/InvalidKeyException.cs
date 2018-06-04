using System;

namespace ApiGateway.Common.Exceptions
{
    public class InvalidKeyException : ApiGatewayException
    {
        public InvalidKeyException(string message) : base(message)
        {
            ErrorCode = 4001;
        }
    }
}