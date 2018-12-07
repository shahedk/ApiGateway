using System.Net;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace ApiGateway.Common.Extensions
{
    public static class UriHelper
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
            var apiNameFromPath = "";
            var path = GetPath(request);
            
            var tokens = path.Split("/");

            if (tokens[0] == "api")
            {
                // Application API   
                if (tokens.Length > 2)
                {
                    // eg. http://api/service-name/api-name/{id}
                    apiNameFromPath = tokens[2];
                }
            }
            else if (tokens[0] == AppConstants.SysApiServiceName)
            {
                // System API
                if (tokens.Length > 2)
                {
                    // eg. http://sys/api-name/action/{id}
                    apiNameFromPath = tokens[1] + "/" + tokens[2];
                }
                else if (tokens.Length > 1)
                {
                    // eg. http://sys/api-name/{id}
                    apiNameFromPath = tokens[1];
                }
            }
            else
            {
                // Unknown Url format
               throw new ApiGatewayException("Unknown Url format", HttpStatusCode.NotFound);
            }
            
            return apiNameFromPath;
        }

        public static string GetServiceName(this HttpRequest request)
        {
            var serviceName = "";
            var tokens = GetPath(request).Split("/");
            if (tokens.Length > 1)
            {
                // http://url/service-name/api-name/id
                if (tokens[0].ToLower() == AppConstants.SysApiServiceName)
                {
                    serviceName = tokens[0];
                }
                else if (tokens[0].ToLower() == "api")
                {
                    serviceName = tokens[1];
                }

            }

            return serviceName;
        }
    }
}