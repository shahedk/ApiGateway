using System;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Models;
using ApiGateway.Core.KeyValidators;
using ApiGateway.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace ApiGateway.Core
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IStringLocalizer<ApiKeyValidator> _localizer;
        private readonly IApiData _apiData;
        private readonly IKeyData _keyData;

        public ApiKeyValidator(IServiceProvider serviceProvider, IStringLocalizer<ApiKeyValidator> localizer, IApiData apiData, IKeyData keyData)
        {
            _serviceProvider = serviceProvider;
            _localizer = localizer;
            _apiData = apiData;
            _keyData = keyData;
        }

        public async Task<KeyValidationResult> IsValid(KeyModel clientKey, KeyModel serviceKey, string httpMethod, string serviceId, string apiUrl)
        {
            var serviceKeyResult = await IsKeyValid(serviceKey.Id, clientKey);
            if (!serviceKeyResult.IsValid)
            {
                // Service key validation failed
                return new KeyValidationResult()
                {
                    InnerValidationResult = serviceKeyResult,
                    IsValid = false,
                    Message = _localizer["Service key validation failed"]
                };
            }
            else
            {
                var clientKeyResult = await IsKeyValid(serviceKey.Id, clientKey);
                if (!clientKeyResult.IsValid)
                {
                    // Client key validation failed
                    return new KeyValidationResult()
                    {
                        InnerValidationResult = clientKeyResult,
                        IsValid = false,
                        Message = _localizer["Client key validation failed"]
                    };
                }
                else
                {
                    // Both keys are valid. Now check if client has the right permission to access the api/url
                    var result = new KeyValidationResult();
                    var api = await _apiData.Get(serviceKey, serviceId, httpMethod, apiUrl);
                    foreach (var role in api.ApiInRole)
                    {
                        result.IsValid = clientKey.Roles.Exists(x => x.Id == role.Id);
                        if (result.IsValid)
                        {
                            break;
                        }
                    }

                    return result;
                }
            }

        }

        public Task<KeyValidationResult> IsAllowedToManageApiGateway(KeyModel clientKey)
        {
            // 1. Check if key=secret valid

            // 2. Check if right permissions set

            throw new System.NotImplementedException();
        }
        

        private async Task<KeyValidationResult> IsKeyValid(string ownerKeyId, KeyModel key)
        {
            KeyValidationResult result;
            if (key.Type == ApiKeyTypes.ClientSecret)
            {
                var keyValidator = _serviceProvider.GetService<KeySecretValidator>();
                result = await keyValidator.IsValid(ownerKeyId, key.PublicKey, key.Properties[ApiKeyPropertyNames.ClientSecret]);
            }
            else
            {
                throw new InvalidKeyException("Unknown key type.");
            }

            return result;
        }
    }
}