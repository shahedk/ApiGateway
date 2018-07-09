using System;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Core.Test;
using ApiGateway.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace ApiGateway.WebApi.Test
{
    public class KeyTest : WebApiTestBase
    {
        protected async Task<KeyController> GetKeyController()
        {
            var rootKey = await GetRootKey();
            var requestHelper = new MockApiRequestHelper(rootKey.PublicKey);
            var keyController = new KeyController(await GetKeyData(), requestHelper);

            return keyController;
        }

        [Fact]
        public async Task<KeyModel> Create()
        {
            var rootKey = await GetRootKey();
            var keyController = await GetKeyController();

            var key = new KeyModel()
            {
                OwnerKeyId = rootKey.Id,
                PublicKey = ModelHelper.GeneratePublicKey(),
                Type = ApiKeyTypes.ClientSecret
            };

        
            var savedKey = await keyController.Post(key);

            Assert.Equal(key.PublicKey, savedKey.PublicKey);

            return savedKey;
        }

        [Fact]
        public async Task Update()
        {
            var keyController = await GetKeyController();
            var existingKey = await Create();

            var model = new KeyModel()
            {
                Id = existingKey.Id,
                PublicKey = existingKey.PublicKey,
                IsDisabled = existingKey.IsDisabled,
                OwnerKeyId = existingKey.OwnerKeyId,
                Properties = existingKey.Properties,
                CreateDate = existingKey.CreateDate,
                Type = ApiKeyTypes.JwtToken
            };

            var saved = await keyController.Put(existingKey.Id, model);

            Assert.Equal(saved.Type, ApiKeyTypes.JwtToken);
        }

        [Fact]
        public async Task Get()
        {
            var keyController = await GetKeyController();
            var existingKey = await Create();

            var saved  = await keyController.Get(existingKey.Id);

            Assert.Equal(saved.PublicKey, existingKey.PublicKey);
        }

        [Fact]
        public async Task Delete()
        {
            var keyController = await GetKeyController();
            var existingKey = await Create();

            await keyController.Delete(existingKey.Id);

            try
            {
                var saved = await keyController.Get(existingKey.Id);
                Assert.Null(saved);
            }
            catch (Exception ex)
            {
                Assert.True( ex is ItemNotFoundException);
            }

            
        }
    }
}
