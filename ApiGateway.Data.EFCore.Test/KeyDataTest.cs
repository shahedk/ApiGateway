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
        public async Task CreateKey()
        {
            var data = await GetKeyData();

            var key = new KeyModel()
            {
                PublicKey = ModelHelper.GeneratePublicKey(),
                Type = ApiKeyTypes.ClientSecret
            };

            var savedKey = await data.Create(string.Empty, key);

            Assert.Equal(key.PublicKey, savedKey.PublicKey);
        }

        [Fact]
        public async Task UpdateKey()
        {
            var data = await GetKeyData();

            // Create new key
            var key = new KeyModel()
            {
                PublicKey = ModelHelper.GeneratePublicKey(),
                Type = ApiKeyTypes.ClientSecret
            };

            var savedKey = await data.Create(string.Empty, key);

            Assert.Equal(key.PublicKey, savedKey.PublicKey);

            // Update key
            savedKey.PublicKey = ModelHelper.GeneratePublicKey();
            await data.Update(string.Empty, savedKey);

            var updatedKey = await data.Get(string.Empty, savedKey.Id);
            
            Assert.Equal(updatedKey.PublicKey, savedKey.PublicKey);
        }

        [Fact]
        public async Task DeleteKey()
        {
            var data = await GetKeyData();

            // Create new key
            var key = new KeyModel()
            {
                PublicKey = ModelHelper.GeneratePublicKey(),
                Type = ApiKeyTypes.ClientSecret
            };

            var savedKey = await data.Create(string.Empty, key);

            Assert.Equal(key.PublicKey, savedKey.PublicKey);

            // Delete
            await data.Delete(string.Empty, savedKey.Id);

            try
            {
                var x = await data.GetByPublicKey(savedKey.PublicKey);

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