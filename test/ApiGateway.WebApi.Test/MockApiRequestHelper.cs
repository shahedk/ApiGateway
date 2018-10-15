using ApiGateway.Client;
using ApiGateway.Common.Constants;

namespace ApiGateway.WebApi.Test
{
    public class MockApiRequestHelper : IApiRequestHelper
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;

       
        public MockApiRequestHelper(string apiKey, string apiSecret)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public string GetApiKey()
        {
            return _apiKey;
        }

        public string GetApiSecret()
        {
            return _apiSecret;
        }


        public string GetApiKeyType()
        {
            return ApiKeyTypes.ClientSecret;
        }

        public string GetServiceApiKeyType()
        {
            throw new System.NotImplementedException();
        }

        public string GetApiName()
        {
            throw new System.NotImplementedException();
        }

        public string GetServiceName()
        {
            throw new System.NotImplementedException();
        }
    }
}