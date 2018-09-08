using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.Extensions;
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

        
        public async Task<KeyModel> Create(KeyModel model)
        {
            var entity = model.ToEntity();
 
            // Save
            _context.Keys.Add(entity);
            await _context.SaveChangesAsync();

            return entity.ToModel();
        }

        public async Task<KeyModel> Update(KeyModel model)
        {
            var ownerKey = int.Parse(model.OwnerKeyId);
            var entityId = int.Parse(model.Id);
            var existing = await _context.Keys.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKey && x.Id == entityId);

            // Update properties
            existing.OwnerKeyId = int.Parse(model.OwnerKeyId);
            existing.PublicKey = model.PublicKey;
            existing.IsDisabled = model.IsDisabled;
            existing.Properties = model.Properties.ToJson();
            existing.Type = model.Type;
            existing.ModifiedDate = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();

            return existing.ToModel();
        }

        public async Task Delete(string ownerKeyId, string id)
        {
            var ownerKey = int.Parse(ownerKeyId);
            var entityId = int.Parse(id);
            var existing = await _context.Keys.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKey && x.Id == entityId);

            _context.Keys.Remove(existing);
            await _context.SaveChangesAsync();
        }

        public async Task<KeyModel> Get(string ownerKeyId, string keyId)
        {
            var ownerKey = int.Parse(ownerKeyId);
            var id = int.Parse(keyId);
            var entity = await _context.Keys.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKey && x.Id == id);

            if (entity == null)
            {
                return null;
            }
            else
            {
                var roles = await _context.KeyInRoles.Where(x => x.KeyId == entity.Id).Select(x => x.Role.ToModel())
                    .ToListAsync();

                return entity.ToModel(roles);
            }
        }

        public async Task<IList<KeyModel>> GetAll(string ownerKeyId)
        {
            
            var ownerKey = int.Parse(ownerKeyId);
            var list = await _context.Keys.Where(x => x.OwnerKeyId == ownerKey).Select(x=>x.ToModel()).ToListAsync();

            return list;
        }

        public async Task<KeyModel> GetByPublicKey(string publicKey)
        {
            var entity = await _context.Keys.SingleOrDefaultAsync(x => x.PublicKey == publicKey);

            if (entity == null) return null;

            var roles = await _context.KeyInRoles.Where(x => x.KeyId == entity.Id).Select(x => x.Role.ToModel()).ToListAsync();

            return entity.ToModel(roles);
        }

        public async Task<int> Count()
        {
            var count = await _context.Keys.CountAsync();

            return count;
        }
    }
}