using ApiGateway.Client;
using ApiGateway.Common.Constants;

namespace ApiGateway.WebApi.Test
{
    public class MockApiRequestHelper : IApiRequestHelper
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly string _serviceKey;
        private readonly string _serviceSecret;

       
        public MockApiRequestHelper(string apiKey, string apiSecret)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public MockApiRequestHelper(string apiKey, string apiSecret, string serviceKey, string serviceSecret)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _serviceKey = serviceKey;
            _serviceSecret = serviceSecret;
        }

        public string GetApiKey()
        {
            return _apiKey;
        }

        public string GetApiSecret()
        {
            return _apiSecret;
        }

        public string GetServiceApiKey()
        {
            return _serviceKey;
        }

        public string GetServiceApiSecret()
        {
            return _serviceSecret;
        }

        public string GetApiKeyType()
        {
            return ApiKeyTypes.ClientSecret;
        }

        public string GetServiceApiKeyType()
        {
            return ApiKeyTypes.ClientSecret;
        }
    }
}