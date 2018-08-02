using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class AccessRuleData : IAccessRuleData
    {
        public Task<AccessRuleModel> Create(AccessRuleModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<AccessRuleModel> Update(AccessRuleModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string ownerKeyId, string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<AccessRuleModel> Get(string ownerKeyId, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}