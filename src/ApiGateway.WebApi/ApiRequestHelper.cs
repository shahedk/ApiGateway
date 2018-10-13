using ApiGateway.Client;
using ApiGateway.Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace ApiGateway.WebApi
{
    public class ApiRequestHelper : IApiRequestHelper
    {
        private readonly IHttpContextAccessor _accessor;

        public ApiRequestHelper(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        private string GetValue(string key)
        {
            _accessor.HttpContext.Request.Headers.TryGetValue(key, out var val);

            return val.ToString();
        }

        public string GetApiKey()
        {
            return GetValue(ApiHttpHeaders.ApiKey);
        }

        public string GetApiSecret()
        {
            return GetValue(ApiHttpHeaders.ApiSecret);
        }
        
        public string GetApiKeyType()
        {
            var type= GetValue(ApiHttpHeaders.KeyType);

            if (string.IsNullOrEmpty(type))
            {
                return ApiKeyTypes.ClientSecret;
            }
            else
            {
                return type;
            }
        }

        public string GetServiceApiKeyType()
        {
            return GetValue(ApiHttpHeaders.KeyType) ?? ApiKeyTypes.ClientSecret;
        }
    }
}