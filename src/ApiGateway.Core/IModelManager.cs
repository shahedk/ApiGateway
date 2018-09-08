using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGateway.Core
{
    public interface IManager<T>
    {
        Task<T> Create(string ownerPublicKey, T model);
        Task<T> Update(string ownerPublicKey, T model);
        Task Delete(string ownerPublicKey, string id);
        Task<T> Get(string ownerPublicKey, string id);
        Task<IList<T>> GetAll(string ownerPublicKey);
    }
}