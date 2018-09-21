using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Core
{
    public interface IKeyManager : IManager<KeyModel>
    {
        Task<KeyModel> ReGenerateSecret(string ownerPublicKey, string keyPublicKey);
        Task<KeyModel> GetByPublicKey(string publicKey);
        Task<KeyModel> CreateRootKey();
    }
}