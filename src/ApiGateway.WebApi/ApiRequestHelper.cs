using ApiGateway.Client;
using ApiGateway.Common.Constants;
using Microsoft.AspNetCore.Http;

namespace ApiGateway.WebApi
{
    public class ApiRequestHelper : IApiRequestHelper
    {
        private readonly IHttpContextAccessor _accessor;

        public ApiRequestHelper(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public string GetApiKey()
        {
            var apiKey = _accessor.HttpContext.Items[ApiHttpHeaders.ApiKey] as string;

            return apiKey;
        }

        public string GetApiSecret()
        {
            var apiKey = _accessor.HttpContext.Items[ApiHttpHeaders.ApiSecret] as string;

            return apiKey;
        }
        
        public string GetServiceApiKey()
        {
            var apiKey = _accessor.HttpContext.Items[ApiHttpHeaders.ServiceApiKey] as string;

            return apiKey;
        }

        public string GetServiceApiSecret()
        {
            var apiKey = _accessor.HttpContext.Items[ApiHttpHeaders.ServiceApiSecret] as string;

            return apiKey;
        }

        public string GetApiKeyType()
        {
            var type = ApiKeyTypes.ClientSecret; // Default 

            var keyType = _accessor.HttpContext.Items[ApiHttpHeaders.KeyType] as string;

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