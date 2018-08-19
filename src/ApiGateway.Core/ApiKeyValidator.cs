using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Core.KeyValidators;
using ApiGateway.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Core
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly KeySecretValidator _keySecretValidator;
        private readonly IStringLocalizer<ApiKeyValidator> _localizer;
        private readonly ILogger<ApiKeyValidator> _logger;
        private readonly IApiManager _apiManager;
        private readonly IKeyManager _keyManager;

        public ApiKeyValidator(KeySecretValidator keySecretValidator, IStringLocalizer<ApiKeyValidator> localizer, ILogger<ApiKeyValidator> logger, IApiManager apiManager, IKeyManager keyManager)
        {
            _keySecretValidator = keySecretValidator;
            _localizer = localizer;
            _logger = logger;
            _apiManager = apiManager;
            _keyManager = keyManager;
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
                    var api = await _apiManager.Get(serviceKey.PublicKey, serviceId, httpMethod, apiUrl);
                    var clientKeyWithRoles = await _keyManager.GetByPublicKey(clientKey.PublicKey);
                    foreach (var role in api.Roles)
                    {
                        result.IsValid = clientKeyWithRoles.Roles.SingleOrDefault(x => x.Id == role.Id) != null;
                        if (result.IsValid)
                        {
                            break;
                        }
                    }

                    return result;
                }
            }
        }

        private async Task<KeyValidationResult> IsKeyValid(string ownerKeyId, KeyModel key)
        {
            KeyValidationResult result;
            if (key.Type == ApiKeyTypes.ClientSecret)
            {
                result = await _keySecretValidator.IsValid(ownerKeyId, key.PublicKey, key.GetSecret());
            }
            else
            {
                var msg = _localizer["Unknown key type."];
                throw new InvalidKeyException(msg, HttpStatusCode.Unauthorized);
            }

            return result;
        }
    }
}