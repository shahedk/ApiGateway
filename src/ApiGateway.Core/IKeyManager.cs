using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Core
{
    public interface IKeyManager : IManager<KeyModel>
    {
        Task<KeyModel> ReGenerateSecret1(string ownerPublicKey, string keyPublicKey);
        Task<KeyModel> ReGenerateSecret2(string ownerPublicKey, string keyPublicKey);
        Task<KeyModel> ReGenerateSecret3(string ownerPublicKey, string keyPublicKey);
        Task<KeyModel> GetByPublicKey(string publicKey);
        Task<KeyModel> CreateRootKey();

        Task<IList<KeySummaryModel>> GetAllSummary(string ownerPublicKey);
    }
}