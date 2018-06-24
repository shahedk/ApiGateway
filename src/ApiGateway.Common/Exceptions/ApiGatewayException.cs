using System;
using System.Net;

namespace ApiGateway.Common.Exceptions
{
    public class ApiGatewayException : Exception
    {
        public HttpStatusCode ErrorCode { get; set; }

        public ApiGatewayException(string message, HttpStatusCode errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
        
    }
}