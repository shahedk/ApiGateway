﻿using System;
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
            string serviceId, string apiUrl, string httpMethod)
        {
            // throw new NotImplementedException();
            
            var validationResult = new KeyValidationResult();

            var clientKey = new KeyModel
            {
                Type = _apiRequestHelper.GetApiKeyType(),
                PublicKey = _apiRequestHelper.GetApiKey(),
                Properties = {[ApiKeyPropertyNames.ClientSecret] = _apiRequestHelper.GetApiSecret()}
            };

            var serviceKey = new KeyModel
            {
                Type = _apiRequestHelper.GetApiKeyType(),
                PublicKey = _apiRequestHelper.GetServiceApiKey(),
                Properties = {[ApiKeyPropertyNames.ClientSecret] = _apiRequestHelper.GetServiceApiSecret()}
            };

            validationResult = await _keyValidator.IsValid(clientKey, serviceKey, httpMethod, serviceId, apiUrl);

            
            return validationResult;
        }
    }
}
