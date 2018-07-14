using System.Threading.Tasks;
using ApiGateway.Common.Models;
using ApiGateway.Data;

namespace ApiGateway.Core
{
    public class AccessRuleManager : IAccessRuleManager
    {
        private readonly IAccessRuleData _accessRuleData;

        public AccessRuleManager(IAccessRuleData accessRuleData)
        {
            _accessRuleData = accessRuleData;
        }

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