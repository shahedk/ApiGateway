﻿using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.Entity;
using ApiGateway.Data.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Localization.Internal;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class RoleData: IRoleData
    {
        private readonly ApiGatewayContext _context;
        private readonly IStringLocalizer<RoleData> _localizer;
        private readonly ILogger<RoleData> _logger;
        private readonly IKeyData _keyData;

        public RoleData(ApiGatewayContext context, IStringLocalizer<RoleData> localizer, ILogger<RoleData> logger, IKeyData keyData)
        {
            _context = context;
            _localizer = localizer;
            _logger = logger;
            _keyData = keyData;
        }


        public async Task<RoleModel> Create(string ownerPublicKey, RoleModel model)
        {
            var entity = model.ToEntity();

            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();

            return entity.ToModel();
        }

        public Task<RoleModel> Update(string ownerPublicKey, RoleModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<RoleModel> Get(string ownerPublicKey, string id)
        {
            var ownerKey = await _keyData.GetByPublicKey(ownerPublicKey);
            var ownerKeyId = int.Parse(ownerKey.Id);
            var roleId = int.Parse(id);
            var role = await _context.Roles.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId && x.Id == roleId);

            return role.ToModel();
        }

        public async Task AddKeyInRole(string roleOwnerPublicKey, string roleId, string keyPublicKey)
        {
            var ownerKey = await _keyData.GetByPublicKey(roleOwnerPublicKey);
            var key = await _keyData.GetByPublicKey(keyPublicKey);
            var role = await Get(roleOwnerPublicKey, roleId);

            var keyId = int.Parse(key.Id);
            var roleId2 = int.Parse(role.Id);
            var ownerKeyId = int.Parse(ownerKey.Id);

            var exists = await _context.KeyInRoles.SingleOrDefaultAsync(x => x.KeyId == keyId && x.RoleId == roleId2 && x.OwnerKeyId == ownerKeyId);

            if (exists == null)
            {
                var map = new KeyInRole()
                {
                    OwnerKeyId =  int.Parse(role.OwnerKeyId),
                    KeyId = keyId,
                    RoleId = roleId2
                };

                _context.KeyInRoles.Add(map);
                await _context.SaveChangesAsync();
            }
            else
            {
                var msg = _localizer["Api already in role"];
                throw new ApiGatewayException(msg, HttpStatusCode.BadRequest);
            }
        }

        public async Task RemoveKeyFromRole(string roleOwnerPublicKey, string roleId, string keyPublicKey)
        {
            var ownerKey = await _keyData.GetByPublicKey(roleOwnerPublicKey);
            var key = await _keyData.GetByPublicKey(keyPublicKey);
            var role = await Get(roleOwnerPublicKey, roleId);

            var keyId = int.Parse(key.Id);
            var roleId2 = int.Parse(role.Id);
            var ownerKeyId = int.Parse(ownerKey.Id);

            var exists = await _context.KeyInRoles.SingleOrDefaultAsync(x => x.KeyId == keyId && x.RoleId == roleId2 && x.OwnerKeyId == ownerKeyId);

            if (exists == null)
            {
                var msg = _localizer["Api does not exits in role"];
                throw new ApiGatewayException(msg, HttpStatusCode.BadRequest);
            }
            else
            {
                _context.KeyInRoles.Remove(exists);
                await _context.SaveChangesAsync();
            }
        }
    }
}