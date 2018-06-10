using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ApiGateway.Common.Constants;

namespace ApiGateway.Common.Exceptions
{
    public class InvalidKeyException : ApiGatewayException
    {
        public InvalidKeyException(string message, HttpStatusCode errorCode) : base(message,errorCode)
        {
        }
    }
}
