using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class RoleData: IRoleData
    {
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