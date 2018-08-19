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

        private async Task<Role> GetEntity(string ownerKeyId, string id)
        {
            var ownerKeyId2 = int.Parse(ownerKeyId);
            var roleId = int.Parse(id);
            var role = await _context.Roles.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId2 && x.Id == roleId);

            return role;
        }

        public async Task<bool> IsKeyInRole(string ownerKeyId, string roleId, string keyId)
        {
            var role = await Get(ownerKeyId, roleId);
            var keyId2 = int.Parse(keyId);
            var roleId2 = int.Parse(role.Id);
            var ownerKeyId2 = int.Parse(ownerKeyId);

            var exists = await _context.KeyInRoles.SingleOrDefaultAsync(x => x.KeyId == keyId2 && x.RoleId == roleId2 && x.OwnerKeyId == ownerKeyId2);

            return (exists != null);
        }

        public async Task AddKeyInRole(string ownerKeyId, string roleId, string keyId)
        {
            var role = await Get(ownerKeyId, roleId);

            var ownerKeyId2 = int.Parse(ownerKeyId);
            var keyId2 = int.Parse(keyId);
            var roleId2 = int.Parse(role.Id);

            var exists = await _context.KeyInRoles.SingleOrDefaultAsync(x => x.KeyId == keyId2 && x.RoleId == roleId2 && x.OwnerKeyId == ownerKeyId2);

            if (exists == null)
            {
                var map = new KeyInRole()
                {
                    OwnerKeyId = int.Parse(role.OwnerKeyId),
                    KeyId = keyId2,
                    RoleId = roleId2
                };

                _context.KeyInRoles.Add(map);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveKeyFromRole(string ownerKeyId, string roleId, string keyId)
        {
            var ownerKeyId2 = int.Parse(ownerKeyId);
            var keyId2 = int.Parse(keyId);
            var roleId2 = int.Parse(roleId);

            var exists = await _context.KeyInRoles.SingleOrDefaultAsync(x => x.KeyId == keyId2 && x.RoleId == roleId2 && x.OwnerKeyId == ownerKeyId2);

            if (exists != null)
            {
                _context.KeyInRoles.Remove(exists);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsApiInRole(string ownerKeyId, string roleId, string apiId)
        {
            var role = await Get(ownerKeyId, roleId);
            var apiId2 = int.Parse(apiId);
            var roleId2 = int.Parse(role.Id);
            var ownerKeyId2 = int.Parse(ownerKeyId);

            var exists = await _context.ApiInRoles.SingleOrDefaultAsync(x => x.ApiId == apiId2 && x.RoleId == roleId2 && x.OwnerKeyId == ownerKeyId2);

            return (exists != null);
        }

        public async Task AddApiInRole(string ownerKeyId, string roleId, string apiId)
        {
            var apiId2 = int.Parse(apiId);
            var roleId2 = int.Parse(roleId);
            var ownerKeyId2 = int.Parse(ownerKeyId);

            var exists = await _context.ApiInRoles.SingleOrDefaultAsync(x => x.ApiId == apiId2 && x.RoleId == roleId2 && x.OwnerKeyId == ownerKeyId2);

            if (exists == null)
            {
                var map = new ApiInRole()
                {
                    OwnerKeyId =  ownerKeyId2,
                    ApiId = apiId2,
                    RoleId = roleId2
                };

                _context.ApiInRoles.Add(map);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveApiFromRole(string ownerKeyId, string roleId, string apiId)
        {
            var apiId2 = int.Parse(apiId);
            var roleId2 = int.Parse(roleId);
            var ownerKeyId2 = int.Parse(ownerKeyId);

            var exists = await _context.ApiInRoles.SingleOrDefaultAsync(x => x.ApiId == apiId2 && x.RoleId == roleId2 && x.OwnerKeyId == ownerKeyId2);

            if (exists != null)
            {
                _context.ApiInRoles.Remove(exists);
                await _context.SaveChangesAsync();
            }
        }
    }
}