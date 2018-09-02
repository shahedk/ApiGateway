using System;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
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

        public KeyManager( IKeyData keyData,IStringLocalizer<IKeyManager> localizer,ILogger<IKeyManager> logger)
        {
            _keyData = keyData;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<KeyModel> Create(string ownerPublicKey, KeyModel model)
        {
            var ownerKey = await _keyData.GetByPublicKey(ownerPublicKey);
            model.OwnerKeyId = ownerKey.Id;

            var result = await _keyData.Create(model);
            _logger.LogInformation(LogEvents.NewKeyCreated,string.Empty, ownerPublicKey, model.PublicKey);

            return result;
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

        public async Task<KeyModel> GetByPublicKey(string publicKey)
        {
            return await _keyData.GetByPublicKey(publicKey);
        }

        public async Task<KeyModel> CreateRootKey()
        {
            var existingKeyCount = await _keyData.Count();

            if (existingKeyCount > 0)
            {
                var msg = _localizer["Master key can only be created for empty database"];
                throw new InvalidOperationException(msg);
            }

            var model = new KeyModel
            {
                Type = ApiKeyTypes.ClientSecret,
                PublicKey = ModelHelper.GeneratePublicKey(),
                Properties = {[ApiKeyPropertyNames.ClientSecret] = ModelHelper.GenerateSecret()}
            };


            var saved = await _keyData.Create(model);

            return saved;
        }
    }
}