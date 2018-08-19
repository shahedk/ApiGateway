using ApiGateway.Client;
using ApiGateway.Common.Constants;
using Microsoft.AspNetCore.Http;

namespace ApiGateway.WebApi
{
    public class ApiRequestHelper : IApiRequestHelper
    {
        private readonly HttpContext _context;

        public ApiRequestHelper(HttpContext context)
        {
            _context = context;
        }
        public string GetApiKey()
        {
            var apiKey = _context.Items[ApiHttpHeaders.ApiKey] as string;

            return apiKey;
        }

        public string GetApiSecret()
        {
            var apiKey = _context.Items[ApiHttpHeaders.ApiSecret] as string;

            return apiKey;
        }
        
        public string GetServiceApiKey()
        {
            var apiKey = _context.Items[ApiHttpHeaders.ServiceApiKey] as string;

            return apiKey;
        }

        public string GetServiceApiSecret()
        {
            var apiKey = _context.Items[ApiHttpHeaders.ServiceApiSecret] as string;

            return apiKey;
        }

        public string GetApiKeyType()
        {
            var type = ApiKeyTypes.ClientSecret; // Default 

            var keyType = _context.Items[ApiHttpHeaders.KeyType] as string;

            if (keyType == "JWT")
            {
                type = ApiKeyTypes.JwtToken;
            }

            return type;
        }

        public string GetServiceApiKeyType()
        {
            return GetApiKeyType();
        }
    }
}