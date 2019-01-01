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
        private readonly IDistributedCache _cache;

        public KeySecretValidator(IKeyManager keyManager, 
            IStringLocalizer<KeySecretValidator> localizer, 
            ILogger<KeySecretValidator> logger,
            IDistributedCache cache)
        {
            _keyManager = keyManager;
            _localizer = localizer;
            _logger = logger;
            _cache = cache;
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
                if (IsValidOnCache(pubKey, secret, out var keyId))
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
                          key.GetSecret3() == secret ||
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
                        StoreValidationResultCache(pubKey, key.Id, secret);
                    }
                }
            }

            return result;
        }

        private bool IsValidOnCache(string pubkey, string secret, out string keyId)
        {
            keyId = "";
            var value =   _cache.Get(pubkey);
            if (value != null)
            {
                var pubKeyOnCache = new ValidationResultCacheItem(value);

                keyId = pubKeyOnCache.KeyId;
                return pubkey == pubKeyOnCache.PubKey;
            }

            return false;
        }
        
        private async Task StoreValidationResultCache(string pubKey, string keyId, string secret)
        {
            var next3minutes = new DateTimeOffset(DateTime.Now.AddMonths(3));
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(next3minutes);

            var value = new ValidationResultCacheItem(pubKey, keyId).ToBytes();

            // Store validation result
            await _cache.SetAsync(secret, value, options);
            
            //
        }

        class ValidationResultCacheItem
        {
            public string PubKey { get; set; }
            public string KeyId { get; set; }

            public override string ToString()
            {
               return JsonConvert.SerializeObject(this);
            }

            public byte[] ToBytes()
            {
                return Encoding.UTF8.GetBytes(this.ToString());
            }

            public ValidationResultCacheItem(string pubKey, string keyId)
            {
                PubKey = pubKey;
                KeyId = keyId;
            }

            public ValidationResultCacheItem(byte[] bytes)
            {
                var json = Encoding.UTF8.GetString(bytes);
                var val = JsonConvert.DeserializeObject<ValidationResultCacheItem>(json);
                PubKey = val.PubKey;
                KeyId = val.KeyId;
            }
        }
    }
}