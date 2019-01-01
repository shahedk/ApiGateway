using System.Text;
using Newtonsoft.Json;

namespace ApiGateway.Core.KeyValidators
{
    public class KeySecretValidationResultCacheItem
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

        public KeySecretValidationResultCacheItem(string pubKey, string keyId)
        {
            PubKey = pubKey;
            KeyId = keyId;
        }

        public KeySecretValidationResultCacheItem(byte[] bytes)
        {
            var json = Encoding.UTF8.GetString(bytes);
            var val = JsonConvert.DeserializeObject<KeySecretValidationResultCacheItem>(json);
            PubKey = val.PubKey;
            KeyId = val.KeyId;
        }
    }
}