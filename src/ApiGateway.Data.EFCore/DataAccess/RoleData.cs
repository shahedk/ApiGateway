using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.Entity;
using ApiGateway.Data.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class RoleData: IRoleData
    {
        private readonly ApiGatewayContext _context;

        public RoleData(ApiGatewayContext context)
        {
            _context = context;
        }


        public async Task<RoleModel> Create(RoleModel model)
        {
            var entity = model.ToEntity();

            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();

            return entity.ToModel();
        }

        public async Task<RoleModel> Update(RoleModel model)
        {
            var roleId = int.Parse(model.Id);
            var ownerKeyId = int.Parse(model.OwnerKeyId);

            var existing = await _context.Roles.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId && x.Id == roleId);

            existing.Name = model.Name;
            existing.ServiceId = int.Parse(model.ServiceId);

            await _context.SaveChangesAsync();

            return existing.ToModel();
        }

        public async Task Delete(string ownerKeyId, string id)
        {
            var existing = await GetEntity(ownerKeyId, id);

            _context.Roles.Remove(existing);
            await _context.SaveChangesAsync();
        }

        public async Task<RoleModel> Get(string ownerKeyId, string id)
        {
            var role = await GetEntity(ownerKeyId, id);

            return role.ToModel();
        }

        public async Task<IList<RoleModel>> GetAll(string ownerKeyId)
        {
            
            var ownerKey = int.Parse(ownerKeyId);
            var list = await _context.Roles.Where(x => x.OwnerKeyId == ownerKey).Select(x=>x.ToModel()).ToListAsync();

            return list;
        }

        private async Task<Role> GetEntity(string ownerKeyId, string id)
        {
            var ownerKeyId2 = int.Parse(ownerKeyId);
            var roleId = int.Parse(id);
            var role = await _context.Roles.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId2 && x.Id == roleId);

            return role;
        }

        public async Task<bool> IsKeyInRole(string ownerKeyId, string roleId, string keyId)
        {
            var role = await GetEntity(ownerKeyId, roleId);
            if(role == null) throw new InvalidDataException();
            
            var keyId2 = int.Parse(keyId);
            
            var exists = await _context.KeyInRoles.SingleOrDefaultAsync(x => x.KeyId == keyId2 && x.RoleId == role.Id);

            return exists != null;
        }

        public async Task AddKeyInRole(string ownerKeyId, string roleId, string keyId)
        {
            var role = await GetEntity(ownerKeyId, roleId);
            if(role == null) throw new InvalidDataException();
            
            var keyId2 = int.Parse(keyId);
            
            var exists = await _context.KeyInRoles.SingleOrDefaultAsync(x => x.KeyId == keyId2 && x.RoleId == role.Id);

            if (exists == null)
            {
                var map = new KeyInRole
                {
                    KeyId = keyId2,
                    RoleId = role.Id
                };

                _context.KeyInRoles.Add(map);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveKeyFromRole(string ownerKeyId, string roleId, string keyId)
        {
            var keyId2 = int.Parse(keyId);
            var role = await GetEntity(ownerKeyId, roleId);
            if(role == null) throw new InvalidDataException();

            var exists = await _context.KeyInRoles.SingleOrDefaultAsync(x => x.KeyId == keyId2 && x.RoleId == role.Id);

            if (exists != null)
            {
                _context.KeyInRoles.Remove(exists);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsApiInRole(string ownerKeyId, string roleId, string apiId)
        {
            var apiId2 = int.Parse(apiId);
            var role = await GetEntity(ownerKeyId, roleId);
            if(role == null) throw new InvalidDataException();

            var exists = await _context.ApiInRoles.SingleOrDefaultAsync(x => x.ApiId == apiId2 && x.RoleId == role.Id);

            return exists != null;
        }

        public async Task AddApiInRole(string ownerKeyId, string roleId, string apiId)
        {
            var apiId2 = int.Parse(apiId);
            var role = await GetEntity(ownerKeyId, roleId);
            if(role == null) throw new InvalidDataException();

            var ownerKeyId2 = int.Parse(ownerKeyId);

            var exists = await _context.ApiInRoles.SingleOrDefaultAsync(x => x.ApiId == apiId2 && x.RoleId == role.Id);

            if (exists == null)
            {
                var map = new ApiInRole
                {
                    ApiId = apiId2,
                    RoleId = role.Id
                };

                _context.ApiInRoles.Add(map);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveApiFromRole(string ownerKeyId, string roleId, string apiId)
        {
            var apiId2 = int.Parse(apiId);
            
            var role = await GetEntity(ownerKeyId, roleId);
            if(role == null) throw new InvalidDataException();

            var exists = await _context.ApiInRoles.SingleOrDefaultAsync(x => x.ApiId == apiId2 && x.RoleId == role.Id);

            if (exists != null)
            {
                _context.ApiInRoles.Remove(exists);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> Count(string ownerKeyId, string serviceId, bool isDisabled)
        {
            var serviceId2 = int.Parse(serviceId);
            var ownerKeyId2 = int.Parse(ownerKeyId);
            
            return await _context.Roles.CountAsync(x =>
                x.ServiceId == serviceId2 && x.OwnerKeyId == ownerKeyId2 && x.IsDisabled == isDisabled);
        }
    }
}