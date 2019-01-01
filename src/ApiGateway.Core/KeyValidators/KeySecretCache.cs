using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Distributed;

namespace ApiGateway.Core.KeyValidators
{
    public class KeySecretCache
    {
        private readonly IDistributedCache _cache;

        public KeySecretCache(IDistributedCache cache)
        {
            _cache = cache;
        }
        
        public bool IsValidOnCache(string pubkey, string secret, out string keyId)
        {
            keyId = "";
            var value =   _cache.Get(pubkey);
            if (value != null)
            {
                var pubKeyOnCache = new KeySecretValidationResultCacheItem(value);

                keyId = pubKeyOnCache.KeyId;
                return pubkey == pubKeyOnCache.PubKey;
            }

            return false;
        }

        public void RemoveCache(string pubKey, string secret)
        {
            _cache.Remove(secret);    
        }
        
        public async Task StoreValidationResultCache(string pubKey, string keyId, string secret)
        {
            var next3minutes = new DateTimeOffset(DateTime.Now.AddMonths(3));
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(next3minutes);

            var value = new KeySecretValidationResultCacheItem(pubKey, keyId).ToBytes();

            // Store validation result
            await _cache.SetAsync(secret, value, options);
        }
    }
}