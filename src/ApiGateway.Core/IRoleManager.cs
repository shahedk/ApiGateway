using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Core
{
    public interface IRoleManager : IManager<RoleModel>
    {
        Task AddKeyInRole(string roleOwnerPublicKey, string roleId, string keyPublicKey);
        Task RemoveKeyFromRole(string roleOwnerPublicKey, string roleId, string keyPublicKey);

        Task AddApiInRole(string roleOwnerPublicKey, string roleId, string apiId);
        Task RemoveApiFromRole(string roleOwnerPublicKey, string roleId, string apiId);

        Task<int> CountByService(string roleOwnerPublicKey, string serviceId, bool isDisabled);
        
        Task<IList<RoleSummaryModel>> GetAllSummary(string ownerPublicKey);
    }
}