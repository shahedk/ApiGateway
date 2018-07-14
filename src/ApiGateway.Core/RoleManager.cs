using System.Threading.Tasks;
using ApiGateway.Common.Models;
using ApiGateway.Data;

namespace ApiGateway.Core
{
    public class RoleManager : IRoleManager
    {
        private readonly IRoleData _roleData;

        public RoleManager(IRoleData roleData)
        {
            _roleData = roleData;
        }

        public Task<RoleModel> Create(string ownerPublicKey, RoleModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<RoleModel> Update(string ownerPublicKey, RoleModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<RoleModel> Get(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}