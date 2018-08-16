using ApiGateway.Client;

namespace ApiGateway.WebApi.Test
{
    public class MockApiRequestHelper : IApiRequestHelper
    {
        private readonly string _publicKey;

        public MockApiRequestHelper(string publicKey)
        {
            _publicKey = publicKey;
        }
        public string GetApiKey()
        {
            return _publicKey;
        }

        public string GetApiSecret()
        {
            throw new System.NotImplementedException();
        }

        public string GetServiceApiKey()
        {
            throw new System.NotImplementedException();
        }

        public string GetServiceApiSecret()
        {
            throw new System.NotImplementedException();
        }
    }
}