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
        public string GetApiPublicKey()
        {
            return _publicKey;
        }
    }
}