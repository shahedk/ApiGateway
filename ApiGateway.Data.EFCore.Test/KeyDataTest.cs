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
            var ownerKey = await GetOwnerKey();
            var data = await GetKeyData();

            var key = new KeyModel()
            {
                OwnerKeyId = ownerKey.Id,
                PublicKey = ModelHelper.GeneratePublicKey(),
                Type = ApiKeyTypes.ClientSecret
            };

            var savedKey = await data.Create(ownerKey.PublicKey, key);

            Assert.Equal(key.PublicKey, savedKey.PublicKey);

            return await data.GetByPublicKey(key.PublicKey);
        }

        [Fact]
        public async Task UpdateKey()
        {
            var data = await GetKeyData();
            var savedKey = await CreateKey();

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
            var savedKey = await CreateKey(); 

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

        [Fact]
        public async Task AddRoles()
        {
            var serviceData = await GetServiceData();
            var keyData = await GetKeyData();
            var roleData = await GetRoleData();
            var userKey = await CreateKey();
            var systemKey = await GetOwnerKey();

            var serviceModel = new ServiceModel(){Name = "TestService", OwnerKeyId = userKey.Id};
            var savedService = await serviceData.Create(userKey.PublicKey, serviceModel);
            
            var roleModel = new RoleModel(){Name = "TestRole", OwnerKeyId = userKey.Id, ServiceId = savedService.Id};
            var savedRole = await roleData.Create(userKey.PublicKey, roleModel);

            userKey.Roles.Add(savedRole);
            await keyData.Update(systemKey.PublicKey, userKey);

            var savedKey = await keyData.GetByPublicKey(systemKey.PublicKey);

            Assert.True(savedKey.Roles.Count == 1);
            Assert.Equal(savedKey.Roles[0].Name , roleModel.Name);
        }
    }
}