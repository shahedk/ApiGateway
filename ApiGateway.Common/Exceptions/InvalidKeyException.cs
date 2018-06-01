using System;

namespace ApiGateway.Common.Exceptions
{
    public class InvalidKeyException : ApiGatewayException
    {
        public InvalidKeyException(string message) : base(message)
        {
            ErrorCode = 4001;
        }

        public InvalidKeyException() : base("Invalid key!")
        {
            ErrorCode = 4001;
        }
    }
}