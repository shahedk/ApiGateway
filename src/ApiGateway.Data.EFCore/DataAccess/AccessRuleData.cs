using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class AccessRuleData : IAccessRuleData
    {
        public Task<AccessRuleModel> Create(string ownerPublicKey, AccessRuleModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<AccessRuleModel> Update(string ownerPublicKey, AccessRuleModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<AccessRuleModel> Get(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}