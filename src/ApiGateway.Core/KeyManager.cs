using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Core.KeyValidators;
using ApiGateway.Data;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Core
{
    public class KeyManager : IKeyManager
    {
        private readonly IKeyData _keyData;
        private readonly IStringLocalizer<IKeyManager> _localizer;
        private readonly ILogger<IKeyManager> _logger;
        private readonly IRoleData _roleData;
        private readonly KeySecretCache _keySecretCache;

        public KeyManager(IKeyData keyData,
            IStringLocalizer<IKeyManager> localizer,
            ILogger<IKeyManager> logger,
            IRoleData roleData,
            KeySecretCache keySecretCache)
        {
            _keyData = keyData;
            _localizer = localizer;
            _logger = logger;
            _roleData = roleData;
            _keySecretCache = keySecretCache;
        }

        public async Task<KeyModel> Create(string ownerPublicKey, KeyModel model)
        {
            var existing = await _keyData.GetByPublicKey(model.PublicKey);
            if (existing != null)
            {
                throw new ApiGatewayException("A key with same PublicKey already exists", HttpStatusCode.BadRequest);
            }

            var ownerKey = await _keyData.GetByPublicKey(ownerPublicKey);
            model.OwnerKeyId = ownerKey.Id;

            var result = await _keyData.Create(model);

            await ReGenerateSecret1(ownerPublicKey, result.PublicKey);
            var keyModel = await ReGenerateSecret2(ownerPublicKey, result.PublicKey);

            _logger.LogInformation(LogEvents.NewKeyCreated,string.Empty, ownerPublicKey, model.PublicKey);

            return keyModel;
        }

        public async Task<KeyModel> Update(string ownerPublicKey, KeyModel model)
        {
            var ownerKey = await _keyData.GetByPublicKey(ownerPublicKey);
            model.OwnerKeyId = ownerKey.Id;

            var result = await _keyData.Update(model);
            _logger.LogInformation(LogEvents.NewKeyUpdated,string.Empty, ownerPublicKey, model.PublicKey);

            return result;
        }

        public async Task Delete(string ownerPublicKey, string id)
        {
            var ownerKey = await _keyData.GetByPublicKey(ownerPublicKey);

            await _keyData.Delete(ownerKey.Id, id);
        }

        public async Task<KeyModel> Get(string ownerPublicKey, string id)
        {
            var ownerKey = await _keyData.GetByPublicKey(ownerPublicKey);
            var model = await _keyData.Get(ownerKey.Id, id);
            if (model == null)
            {
                var msg = _localizer["No key found for the specified owner and Id"];
                throw new ItemNotFoundException(msg);
            }

            return model;
        }

        public async Task<IList<KeyModel>> GetAll(string ownerPublicKey)
        {
            var ownerKey = await GetByPublicKey(ownerPublicKey);
            return await _keyData.GetAll(ownerKey.Id);
        }

        public async Task<KeyModel> ReGenerateSecret1(string ownerPublicKey, string keyPublicKey)
        {
            var ownerKey = await GetByPublicKey(ownerPublicKey);
            var key = await GetByPublicKey(keyPublicKey);

            if ( key != null && ( key.OwnerKeyId == ownerKey.Id || key.Id == ownerKey.Id))
            {
                // Remove old key from cache
                if (key.Properties.ContainsKey(ApiKeyPropertyNames.ClientSecret1))
                {
                    _keySecretCache.RemoveCache(key.PublicKey, key.Properties[ApiKeyPropertyNames.ClientSecret1]);
                }
                
                // owner can reset its secret. And also can reset secret for keys created by it
                key.Properties[ApiKeyPropertyNames.ClientSecret1] = ModelHelper.GenerateSecret();
                return await _keyData.Update(key);
            }
            else
            {
                var msg = _localizer["No key found for the specified owner and Id"];
                throw new InvalidKeyException(msg, HttpStatusCode.NotFound);
            }
        }
        
        public async Task<KeyModel> ReGenerateSecret2(string ownerPublicKey, string keyPublicKey)
        {
            var ownerKey = await GetByPublicKey(ownerPublicKey);
            var key = await GetByPublicKey(keyPublicKey);

            if ( key != null && ( key.OwnerKeyId == ownerKey.Id || key.Id == ownerKey.Id))
            {
                // Remove old key from cache
                if (key.Properties.ContainsKey(ApiKeyPropertyNames.ClientSecret2))
                {
                    _keySecretCache.RemoveCache(key.PublicKey, key.Properties[ApiKeyPropertyNames.ClientSecret2]);
                }

                // owner can reset its secret. And also can reset secret for keys created by it
                key.Properties[ApiKeyPropertyNames.ClientSecret2] = ModelHelper.GenerateSecret();
                return await _keyData.Update(key);
            }
            else
            {
                var msg = _localizer["No key found for the specified owner and Id"];
                throw new InvalidKeyException(msg, HttpStatusCode.NotFound);
            }
        }

        public async Task<KeyModel> ReGenerateSecret3(string ownerPublicKey, string keyPublicKey)
        {
            var ownerKey = await GetByPublicKey(ownerPublicKey);
            var key = await GetByPublicKey(keyPublicKey);

            if ( key != null && ( key.OwnerKeyId == ownerKey.Id || key.Id == ownerKey.Id))
            {
                // Remove old key from cache
                if (key.Properties.ContainsKey(ApiKeyPropertyNames.ClientSecret3))
                {
                    _keySecretCache.RemoveCache(key.PublicKey, key.Properties[ApiKeyPropertyNames.ClientSecret3]);
                }

                // owner can reset its secret. And also can reset secret for keys created by it
                key.Properties[ApiKeyPropertyNames.ClientSecret3] = ModelHelper.GenerateSecret();
                return await _keyData.Update(key);
            }
            else
            {
                var msg = _localizer["No key found for the specified owner and Id"];
                throw new InvalidKeyException(msg, HttpStatusCode.NotFound);
            }
        }

        public async Task<KeyModel> GetByPublicKey(string publicKey)
        {
            return await _keyData.GetByPublicKey(publicKey);
        }

        public async Task<KeyModel> CreateRootKey()
        {
            var existingKeyCount = await _keyData.Count();

            if (existingKeyCount > 0)
            {
                var msg = _localizer["Root key can only be created for empty database"];
                throw new InvalidOperationException(msg);
            }

            var model = new KeyModel
            {
                Type = ApiKeyTypes.ClientSecret,
                PublicKey = "rootkey",
                Properties =
                {
                    [ApiKeyPropertyNames.ClientSecret1] = ModelHelper.GenerateSecret(),
                    [ApiKeyPropertyNames.ClientSecret2] = ModelHelper.GenerateSecret()
                }
            };


            var saved = await _keyData.Create(model);

            return saved;
        }

        public async Task<IList<KeySummaryModel>> GetAllSummary(string ownerPublicKey)
        {
            var list = await GetAll(ownerPublicKey);

            var result = new List<KeySummaryModel>(list.Count);
            foreach (var k in list)
            {
                var key = new KeySummaryModel(k)
                {
                    ActiveRoleCount = await _roleData.CountByKey(k.OwnerKeyId, k.Id, false),
                    DisabledRoleCount = await _roleData.CountByKey(k.OwnerKeyId, k.Id, true)
                };

                result.Add(key);
            }

            return result;
        }
    }
}