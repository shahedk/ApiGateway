using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data
{
    public interface IEntityData<T>
    {
        Task<T> Create(string ownerKeyId, T model);
        Task<T> Update(string ownerKeyId, T model);
        Task Delete(string ownerKeyId, string id);

    }
}