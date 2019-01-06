using System;
using System.IO;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using Xunit;

namespace ApiGateway.Core.Test
{
    public class KeyManagerTest : CoreTestBase
    {
        [Fact]
        public async Task<KeyModel> CreateKey()
        {
            var rootKey = await GetRootKey();
            var manager =  GetKeyManager();

            var key = new KeyModel
            {
                OwnerId = rootKey.Id,
                PublicKey = ModelHelper.GeneratePublicKey(),
                Type = ApiKeyTypes.ClientSecret
            };

            var savedKey = await manager.Create(rootKey.PublicKey, key);

            Assert.Equal(key.PublicKey, savedKey.PublicKey);

            return await manager.GetByPublicKey(key.PublicKey);
        }

        [Fact]
        public async Task UpdateKey()
        {
            var manager =  GetKeyManager();
            var savedKey = await CreateKey();
            var rootKey = await GetRootKey();

            // Update key
            savedKey.PublicKey = ModelHelper.GeneratePublicKey();
            await manager.Update(rootKey.PublicKey, savedKey);

            var updatedKey = await manager.Get(rootKey.PublicKey, savedKey.Id);
            
            Assert.Equal(updatedKey.PublicKey, savedKey.PublicKey);
        }

        [Fact]
        public async Task DeleteKey()
        {
            var data =  GetKeyManager();
            var rootKey = await GetRootKey();

            // Create new key
            var savedKey = await CreateKey(); 

            // Delete
            await data.Delete(rootKey.PublicKey, savedKey.Id);

            try
            {
                var x = await data.GetByPublicKey(savedKey.Id);

                if (x != null)
                {
                    throw new InvalidDataException();
                }
            }
            catch (Exception ex)
            {
                // Expecting this exception
                Assert.True(ex is InvalidKeyException, "Excepting InvalidKeyException");
            }
        }

    }
}