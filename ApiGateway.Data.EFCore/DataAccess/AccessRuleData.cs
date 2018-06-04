using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class AccessRuleData : IAccessRuleData
    {
        public Task<AccessRuleModel> Create(string ownerKeyId, AccessRuleModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<AccessRuleModel> Update(string ownerKeyId, AccessRuleModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string ownerKeyId, string id)
        {
            throw new System.NotImplementedException();
        }
        
    }
}