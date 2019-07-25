using System;
using System.Text;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ApiGateway.Core.KeyValidators
{
    public class KeySecretValidator : IKeyValidator
    {
        private readonly IKeyManager _keyManager;
        private readonly IStringLocalizer<KeySecretValidator> _localizer;
        private readonly ILogger<KeySecretValidator> _logger;
        private readonly KeySecretCache _keySecretCache;
        

        public KeySecretValidator(IKeyManager keyManager, 
            IStringLocalizer<KeySecretValidator> localizer, 
            ILogger<KeySecretValidator> logger,
            KeySecretCache keySecretCache)
        {
            _keyManager = keyManager;
            _localizer = localizer;
            _logger = logger;
            _keySecretCache = keySecretCache;
            
        }

        public async Task<KeyValidationResult> IsValid(string pubKey, string secret)
        {
            var result = new KeyValidationResult();

            if (string.IsNullOrWhiteSpace(pubKey) || string.IsNullOrWhiteSpace(secret))
            {
                result.IsValid = false;
                result.Message = _localizer["Client key or secret cannot be blank"];
            }
            else
            {
                if (_keySecretCache.IsValidOnCache(pubKey, secret, out var keyId))
                {
                    var log = _localizer["Login successful based on Cache for: "] + pubKey;
                    _logger.LogInformation(LogEvents.LoginSuccess, log);

                    result.KeyId = keyId;
                    result.IsValid = true;
                }
                else
                {
                    var key = await _keyManager.GetByPublicKey(pubKey);

                    if (key == null || key.IsDisabled ||
                        !(key.GetSecret1() == secret ||
                          key.GetSecret2() == secret ||
                          key.GetSecret3() == secret))
                    {
                        result.IsValid = false;
                        result.Message = _localizer["Key and/or secret is not valid"];
                    }
                    else
                    {
                        var log = _localizer["Login successful for: "] + pubKey;
                        _logger.LogInformation(LogEvents.LoginSuccess, log);

                        result.KeyId = key.Id;
                        result.IsValid = true;

                        // Store in cache
                        await _keySecretCache.StoreValidationResultCache(pubKey, key.Id, secret);
                    }
                }
            }

            return result;
        }


        
    }
}