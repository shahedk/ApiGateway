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

        public async Task<KeyValidationResult> IsValid(KeyModel clientKey, KeyModel serviceKey, string httpMethod,
            string serviceId, string apiUrl)
        {

            // For non-local api (eg. "/sys/...") service key is required 
            if (!apiUrl.StartsWith(AppConstants.LocalApiUrlPrefix))
            {
                var serviceKeyResult = await IsKeyValid(clientKey);
                if (!serviceKeyResult.IsValid)
                {
                    // Service key validation failed
                    return new KeyValidationResult
                    {
                        InnerValidationResult = serviceKeyResult,
                        IsValid = false,
                        Message = _localizer["Service key validation failed"]
                    };
                }
            }

            // Validate client key
            var clientKeyResult = await IsKeyValid(clientKey);
            if (!clientKeyResult.IsValid)
            {
                // Client key validation failed
                return new KeyValidationResult
                {
                    InnerValidationResult = clientKeyResult,
                    IsValid = false,
                    Message = _localizer["Client key validation failed"]
                };
            }

            // Key validation passed. Now check if client has the right permission to access the api/url
            
            var result = new KeyValidationResult();
            
            var api = await _apiManager.Get(serviceKey.PublicKey, serviceId, httpMethod, apiUrl);
            if (api == null)
            {
                result.Message = _localizer["Api not found"];
                result.IsValid = false;
                return result;
            }

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

        private async Task<KeyValidationResult> IsKeyValid(KeyModel key)
        {
            KeyValidationResult result;
            if (key.Type == ApiKeyTypes.ClientSecret)
            {
                result = await _keySecretValidator.IsValid(key.PublicKey, key.GetSecret());
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