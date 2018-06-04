using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Models;
using ApiGateway.Core.KeyValidators;
using ApiGateway.Data;
using Microsoft.Extensions.Localization;

namespace ApiGateway.Core
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly KeySecretValidator _keySecretValidator;
        private readonly IStringLocalizer<ApiKeyValidator> _localizer;
        private readonly IApiData _apiData;
        private readonly IKeyData _keyData;

        public ApiKeyValidator(KeySecretValidator keySecretValidator, IStringLocalizer<ApiKeyValidator> localizer, IApiData apiData, IKeyData keyData)
        {
            _keySecretValidator = keySecretValidator;
            _localizer = localizer;
            _apiData = apiData;
            _keyData = keyData;
        }

        public async Task<KeyValidationResult> IsValid(KeyModel clientKey, KeyModel serviceKey, string httpMethod, string serviceId, string apiUrl)
        {
            var serviceKeyResult = IsKeyValid(serviceKey);
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
                var clientKeyResult = IsKeyValid(clientKey);
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

        private KeyValidationResult IsKeyValid(KeyModel key)
        {
            KeyValidationResult result;

            switch (key.Type)
            {
                    case ApiKeyTypes.ClientSecret:
                        result = _keySecretValidator.IsValid(key);
                        break;

                    default:
                        throw new InvalidKeyException("Unknown key type.");
            }

            return result;
        }
    }
}