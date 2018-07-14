using System.Threading.Tasks;
using ApiGateway.Common.Models;
using ApiGateway.Data;

namespace ApiGateway.Core
{
    public class KeyManager : IKeyManager
    {
        private readonly IKeyData _keyData;

        public KeyManager( IKeyData keyData)
        {
            _keyData = keyData;
        }

        public Task<KeyModel> Create(string ownerPublicKey, KeyModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<KeyModel> Update(string ownerPublicKey, KeyModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<KeyModel> Get(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}