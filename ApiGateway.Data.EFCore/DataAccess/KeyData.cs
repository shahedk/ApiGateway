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


        public async Task<KeyModel> SaveKey(string ownerKeyId, KeyModel model)
        {
            if (string.IsNullOrEmpty(ownerKeyId))
            {
                throw new InvalidKeyException();
            }

            var existing = await _context.Keys.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId && x.Id == model.Id);

            if (existing != null)
            {
                // Update

                return null;
            }
            
            // Else, Create new

            return null;
        }

        public Task DeleteKey(string ownerKeyId, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}