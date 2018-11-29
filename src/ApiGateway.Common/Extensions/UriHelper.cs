using Microsoft.AspNetCore.Http;

namespace ApiGateway.Common.Extensions
{
    public static class UriHelper
    {
        public static string GetApiName(this HttpRequest request) 
        {
            var apiNameFromPath = "";
            var tokens = request.Path.Value.Split("/");
        
            if (tokens.Length > 2)
            {
                // http://url/service-name/api-name/id
                apiNameFromPath = tokens[2];
            }
            
            return apiNameFromPath;
        }

        public static string GetServiceName(this HttpRequest request)
        {
            var serviceName = "";
            var tokens = request.Path.Value.Split("/");
            if (tokens.Length > 1)
            {
                // http://url/service-name/api-name/id
                
                serviceName = tokens[1];
            }

            return serviceName;
        }
    }
}