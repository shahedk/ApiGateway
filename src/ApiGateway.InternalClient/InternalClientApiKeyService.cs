using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Models;
using ApiGateway.Core;

namespace ApiGateway.InternalClient
{
    public class InternalClientApiKeyService : IClientApiKeyService
    {
        private readonly IApiKeyValidator _keyValidator;
        private readonly IApiRequestHelper _apiRequestHelper;

        public InternalClientApiKeyService(IApiKeyValidator keyValidator, IApiRequestHelper apiRequestHelper)
        {
            _keyValidator = keyValidator;
            _apiRequestHelper = apiRequestHelper;
        }
        
        public async Task<KeyValidationResult> IsClientApiKeyValidAsync(string apiKey, string apiSecret, 
            string serviceName, string apiName, string httpMethod)
        {
            var challenge = new KeyChallenge
            {
                Type = _apiRequestHelper.GetApiKeyType(),
                Properties = new Dictionary<string, string>
                {
                    [ApiKeyPropertyNames.ClientSecret] = apiSecret,
                    [ApiKeyPropertyNames.PublicKey] = apiKey
                }
            };

            var result = await _keyValidator.IsValid(challenge, httpMethod, serviceName, apiName);
            
            return result;
        }
    }
}
