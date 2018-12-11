using System.Net;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace ApiGateway.Client
{
    public static class ClientUriHelper
    {
        private static string GetPath(HttpRequest request)
        {
            var startIndex = request.Path.Value.IndexOf("://");
            var path = request.Path.Value.Substring(startIndex+2).Trim();
         
            if (path.EndsWith("/"))
            {
                // Remove the trailing "/"
                path = path.Substring(0, path.Length - 1);
            }

            return path.ToLower();
        }
        
        public static string GetApiName(this HttpRequest request) 
        {
            var path = GetPath(request);
            
            var tokens = path.Split("/");

            if (tokens.Length > 0)
            {
                if (tokens[0].ToLower() == "api")
                {
                    if (tokens.Length > 1)
                    {
                        return tokens[1];
                    }
                    else
                    {
                        throw new ApiGatewayException("Could not find api name in Url", HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    return tokens[0];    
                }
                
            }
            else
            {
                return path;
            }
        }

    }
}