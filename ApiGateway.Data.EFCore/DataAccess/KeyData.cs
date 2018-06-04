using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class KeyData: IKeyData
    {
        private readonly ApiGatewayContext _context;

        public KeyData(ApiGatewayContext context)
        {
            _context = context;
        }

        public Task<KeyModel> Create(string ownerKeyId, KeyModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<KeyModel> Update(string ownerKeyId, KeyModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string ownerKeyId, string id)
        {
            throw new System.NotImplementedException();
        }


        public Task<KeyModel> Get(string ownerKeyId, string keyId)
        {
            throw new System.NotImplementedException();
        }
    }
}