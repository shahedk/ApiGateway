using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Core;

namespace ApiGateway.InternalClient
{
    public class InternalClientLoginService : IClientLoginService
    {
        private readonly IApiKeyValidator _keyValidator;
        private readonly IApiRequestHelper _apiRequestHelper;

        public InternalClientLoginService(IApiKeyValidator keyValidator, IApiRequestHelper apiRequestHelper)
        {
            _keyValidator = keyValidator;
            _apiRequestHelper = apiRequestHelper;
        }
        
        public async Task<KeyValidationResult> IsClientApiKeyValidAsync(string apiKey, string apiSecret, 
            string serviceName, string apiName, string httpMethod)
        {
            var clientKey = new KeyModel
            {
                Type = _apiRequestHelper.GetApiKeyType(),
                PublicKey = apiKey,
                Properties = {[ApiKeyPropertyNames.ClientSecret1] = apiSecret}
            };

            var result = await _keyValidator.IsValid(clientKey, httpMethod, serviceName, apiName);
            
            return result;
        }
    }
}
