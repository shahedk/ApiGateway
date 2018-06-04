using System;

namespace ApiGateway.Common.Exceptions
{
    public class ApiGatewayException : Exception
    {
        public int ErrorCode { get; set; }
        
        public ApiGatewayException(string message):base(message){}
        
    }
}