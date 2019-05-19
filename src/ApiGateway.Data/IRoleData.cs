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

        Task AddServiceInRole(string ownerKeyId, string roleId, string serviceId);
        Task RemoveServiceFromRole(string ownerKeyId, string roleId, string serviceId);

        Task<int> CountByService(string ownerKeyId, string serviceId, bool isDisabled);
        
        Task<int> CountByKey(string ownerKeyId, string keyId, bool isDisabled);
        
        Task<int> CountByApi(string ownerKeyId, string apiId, bool isDisabled);

        Task<int> ApiCountInRole(string ownerKeyId, string roleId);

        Task<int> KeyCountInRole(string ownerKeyId, string roleId);
    }
}