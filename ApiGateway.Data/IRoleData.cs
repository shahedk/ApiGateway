using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data
{
    public interface IRoleData : IEntityData<RoleModel>
    {
        Task AddKeyInRole(string roleOwnerPublicKey, string roleId, string keyPublicKey);
        Task RemoveKeyFromRole(string roleOwnerPublicKey, string roleId, string keyPublicKey);
    }
}