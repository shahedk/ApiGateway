using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data
{
    public interface IRoleData : IEntityData<RoleModel>
    {
        Task<bool> IsKeyInRole(string ownerKeyId, string roleId, string keyId);
        Task AddKeyInRole(string ownerKeyId, string roleId, string keyId);
        Task RemoveKeyFromRole(string ownerKeyId, string roleId, string keyId);

        Task<bool> IsApiInRole(string ownerKeyId, string roleId, string apiId);
        Task AddApiInRole(string ownerKeyId, string roleId, string apiId);
        Task RemoveApiFromRole(string ownerKeyId, string roleId, string apiId);
    }
}