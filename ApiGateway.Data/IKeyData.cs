using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data
{
    public interface IKeyData
    {
        Task<KeyModel> SaveKey(string ownerKeyId, KeyModel model);
        Task DeleteKey(string ownerKeyId, string id);
        
    }
}