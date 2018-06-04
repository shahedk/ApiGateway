using ApiGateway.Common.Models;

namespace ApiGateway.Core
{
    public interface IApiKeyManager
    {
        // Key
        KeyModel CreateKey(KeyModel clientKey, KeyModel key);
        KeyModel UpdateKey(KeyModel clientKey, KeyModel key);
        void DeleteKey(KeyModel clientKey, string keyId);


    }
}