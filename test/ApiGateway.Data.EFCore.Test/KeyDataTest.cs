using System;
using System.IO;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using Xunit;

namespace ApiGateway.Data.EFCore.Test
{
    public class KeyDataTest : TestBase
    {
        [Fact]
        public async Task<KeyModel> CreateKey()
        {
            var rootKey = await GetRootKey();
            var data = await GetKeyData();

            var key = new KeyModel()
            {
                OwnerKeyId = rootKey.Id,
                PublicKey = ModelHelper.GeneratePublicKey(),
                Type = ApiKeyTypes.ClientSecret
            };

            var savedKey = await data.Create(key);

            Assert.Equal(key.PublicKey, savedKey.PublicKey);

            return await data.GetByPublicKey(key.PublicKey);
        }

        [Fact]
        public async Task UpdateKey()
        {
            var data = await GetKeyData();
            var savedKey = await CreateKey();
            var rootKey = await GetRootKey();

            // Update key
            savedKey.PublicKey = ModelHelper.GeneratePublicKey();
            await data.Update(savedKey);

            var updatedKey = await data.Get(rootKey.Id, savedKey.Id);
            
            Assert.Equal(updatedKey.PublicKey, savedKey.PublicKey);
        }

        [Fact]
        public async Task DeleteKey()
        {
            var data = await GetKeyData();
            var rootKey = await GetRootKey();

            // Create new key
            var savedKey = await CreateKey(); 

            // Delete
            await data.Delete(rootKey.Id, savedKey.Id);

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