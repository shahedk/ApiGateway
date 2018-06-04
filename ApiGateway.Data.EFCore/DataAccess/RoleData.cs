using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class RoleData: IRoleData
    {
        public Task<RoleModel> Create(string ownerKeyId, RoleModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<RoleModel> Update(string ownerKeyId, RoleModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string ownerKeyId, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}