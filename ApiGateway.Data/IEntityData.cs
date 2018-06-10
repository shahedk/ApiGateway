using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data
{
    public interface IEntityData<T>
    {
        Task<T> Create(string ownerPublicKey, T model);
        Task<T> Update(string ownerPublicKey, T model);
        Task Delete(string ownerPublicKey, string id);
        Task<T> Get(string ownerPublicKey, string id);
    }
}