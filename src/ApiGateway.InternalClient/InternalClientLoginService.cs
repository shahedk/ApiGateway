using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Constants;
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
        
        public async Task<KeyValidationResult> IsClientApiKeyValidAsync(string apiKey, string apiSecret, string serviceApiKey, string serviceApiSecret,
            string serviceName, string apiUrl, string httpMethod)
        {
            var validationResult = new KeyValidationResult();

            var clientKey = new KeyModel
            {
                Type = _apiRequestHelper.GetApiKeyType(),
                PublicKey = apiKey,
                Properties = {[ApiKeyPropertyNames.ClientSecret] = apiSecret}
            };

            var serviceKey = new KeyModel
            {
                Type = _apiRequestHelper.GetApiKeyType(),
                PublicKey = serviceApiKey,
                Properties = {[ApiKeyPropertyNames.ClientSecret] = serviceApiSecret}
            };

            validationResult = await _keyValidator.IsValid(clientKey, serviceKey, httpMethod, serviceName, apiUrl);

            
            return validationResult;
        }
    }
}
