using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data
{
    public interface IEntityData<T>
    {
        Task<T> Create(T model);
        Task<T> Update(T model);
        Task Delete(string ownerKeyId, string id);
        Task<T> Get(string ownerKeyId, string id);
    }
}