using System;
using System.Net;
using ApiGateway.Common.Constants;

namespace ApiGateway.Common.Exceptions
{
    public class ItemNotFoundException : ApiGatewayException
    {
        public ItemNotFoundException(string message): base(message, HttpStatusCode.NotFound)
        {

        }

        public ItemNotFoundException(string message, HttpStatusCode errorCode) : base(message,errorCode)
        {
        }
    }
}