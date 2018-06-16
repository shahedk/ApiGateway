using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.Entity;
using ApiGateway.Data.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class KeyData: IKeyData
    {
        private readonly ApiGatewayContext _context;
        private readonly IStringLocalizer<KeyData> _localizer;
        private readonly ILogger<KeyData> _logger;

        public KeyData(ApiGatewayContext context, IStringLocalizer<KeyData> localizer, ILogger<KeyData> logger)
        {
            _context = context;
            _localizer = localizer;
            _logger = logger;
        }

        private async Task UpdateKeyInRoles(int ownerKeyId, Key key, List<RoleModel> roles)
        {
            var changed = false;
            var existingRoles = await _context.KeyInRoles.Where(x => x.KeyId == key.Id).ToListAsync();
            
            // Remove if no longer assigned
            foreach (var existingRole in existingRoles)
            {
                if (!roles.Exists(x => x.Id == existingRole.Id.ToString()))
                {
                    changed = true;
                    _context.KeyInRoles.Remove(existingRole);
                }
            }

            // Add newly assigned roles in list
            foreach (var role in roles)
            {
                if (!existingRoles.Exists(x => x.Id == int.Parse(role.Id)))
                {
                    changed = true;
                    var keyInRole = new KeyInRole {OwnerKeyId = ownerKeyId,  KeyId = key.Id, RoleId = int.Parse(role.Id)};
                    _context.KeyInRoles.Add(keyInRole);
                }
            }

            if (changed)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task<KeyModel> Create(string ownerPublicKey, KeyModel model)
        {
            var entity = model.ToEntity();
            var ownerKey = await GetEntityByPublicKey(ownerPublicKey);

            // Save
            _context.Keys.Add(entity);
            await _context.SaveChangesAsync();

            // Assign roles to key
            var savedKey = await GetEntityByPublicKey(model.PublicKey);
            await UpdateKeyInRoles(ownerKey.Id, savedKey, model.Roles);

            _logger.LogInformation(LogEvents.NewKeyCreated,string.Empty, ownerPublicKey, model.PublicKey);

            return savedKey.ToModel(model.Roles);
        }

        public async Task<KeyModel> Update(string ownerPublicKey, KeyModel model)
        {
            var ownerKey = await GetEntityByPublicKey(ownerPublicKey);
            var existing = await GetEntity(ownerPublicKey, model.Id);

            // Update properties
            existing.PublicKey = model.PublicKey;
            existing.IsDisabled = model.IsDisabled;
            existing.Properties = model.Properties.ToJson();
            existing.Type = model.Type;
            existing.ModifiedDate = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();

            // Update roles assigned to key
            await UpdateKeyInRoles(ownerKey.Id, existing, model.Roles);

            return existing.ToModel(model.Roles);
        }

        public async Task Delete(string ownerPublicKey, string id)
        {
            var existing = await GetEntity(ownerPublicKey, id);

            _context.Keys.Remove(existing);
            await _context.SaveChangesAsync();
        }

        public async Task<Key> GetEntity(string ownerPublicKey, string keyId)
        {
            var ownerKeyId = await GetIdByPublicKey(ownerPublicKey);
            var id = int.Parse(keyId);
            var entity = await _context.Keys.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId && x.Id == id);

            if (entity == null)
            {
                var msg = _localizer["No key found for the specified owner and Id"];
                throw new ItemNotFoundException(msg);
            }

            return entity;
        }

        public async Task<KeyModel> Get(string ownerPublicKey, string keyId)
        {
            var ownerKeyId = await GetIdByPublicKey(ownerPublicKey);
            var id = int.Parse(keyId);
            var entity = await _context.Keys.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId && x.Id == id);

            if (entity == null)
            {
                var msg = _localizer["No key found for the specified owner and Id"];
                throw new ItemNotFoundException(msg);
            }

            return entity.ToModel();
        }

        public async Task<int> GetIdByPublicKey(string publicKey)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                return 0;
            }
            else
            {
                var key = await GetEntityByPublicKey(publicKey);
                return key.Id;
            }
        }

        public async Task<Key> GetEntityByPublicKey(string publicKey)
        {
            var key = await _context.Keys.SingleOrDefaultAsync(x => x.PublicKey == publicKey);

            if (key == null)
            {
                var message = _localizer["Invalid Key"];
                throw new InvalidKeyException(message, HttpStatusCode.Unauthorized);
            }
            else
            {
                return key;
            }
        }

        public async Task<KeyModel> GetByPublicKey(string publicKey)
        {
            var key = await GetEntityByPublicKey(publicKey);
            return key.ToModel();
        }
        

    }
}