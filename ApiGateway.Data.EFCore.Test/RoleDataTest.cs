using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using Xunit;

namespace ApiGateway.Data.EFCore.Test
{
    public class RoleDataTest : TestBase
    {
        
        [Fact]
        public async Task AddKeyInRole()
        {
            var rootKey = await GetRootKey();
            var serviceData = await GetServiceData();
            var keyData = await GetKeyData();
            var roleData = await GetRoleData();

            // # 1. Create key
            
            var data = await GetKeyData();

            var key = new KeyModel()
            {
                OwnerKeyId = rootKey.Id,
                PublicKey = ModelHelper.GeneratePublicKey(),
                Type = ApiKeyTypes.ClientSecret
            };

            var userKey = await data.Create(rootKey.PublicKey, key);

            Assert.Equal(key.PublicKey, userKey.PublicKey);

            // # 2. Create service
            var serviceModel = new ServiceModel(){Name = "TestService", OwnerKeyId = userKey.Id};
            var savedService = await serviceData.Create(userKey.PublicKey, serviceModel);
            
            // # 3. Create role
            var roleModel = new RoleModel(){Name = "TestRole", OwnerKeyId = userKey.Id, ServiceId = savedService.Id};
            var savedRole = await roleData.Create(userKey.PublicKey, roleModel);

            // # 4. Assign role to key
            await roleData.AddKeyInRole(userKey.PublicKey, savedRole.Id, userKey.PublicKey);

            var savedKey = await keyData.GetByPublicKey(userKey.PublicKey);

            Assert.NotNull(savedKey);
            Assert.True(savedKey.Roles.Count == 1);
            Assert.Equal(savedKey.Roles[0].Name , roleModel.Name);
        }

        [Fact]
        public async Task RemoveKeyFromRole()
        {
            var rootKey = await GetRootKey();
            var serviceData = await GetServiceData();
            var keyData = await GetKeyData();
            var roleData = await GetRoleData();

            // # 1. Create key
            
            var data = await GetKeyData();

            var key = new KeyModel()
            {
                OwnerKeyId = rootKey.Id,
                PublicKey = ModelHelper.GeneratePublicKey(),
                Type = ApiKeyTypes.ClientSecret
            };

            var userKey = await data.Create(rootKey.PublicKey, key);

            Assert.Equal(key.PublicKey, userKey.PublicKey);

            // # 2. Create service
            var serviceModel = new ServiceModel(){Name = "TestService", OwnerKeyId = userKey.Id};
            var savedService = await serviceData.Create(userKey.PublicKey, serviceModel);
            
            // # 3. Create role
            var roleModel = new RoleModel(){Name = "TestRole", OwnerKeyId = userKey.Id, ServiceId = savedService.Id};
            var savedRole = await roleData.Create(userKey.PublicKey, roleModel);

            // # 4. Assign role to key
            await roleData.AddKeyInRole(userKey.PublicKey, savedRole.Id, userKey.PublicKey);

            var savedKey = await keyData.GetByPublicKey(userKey.PublicKey);

            Assert.NotNull(savedKey);
            Assert.True(savedKey.Roles.Count == 1);
            Assert.Equal(savedKey.Roles[0].Name , roleModel.Name);

            // # 5. Remove role
            await roleData.RemoveKeyFromRole(userKey.PublicKey, savedRole.Id, userKey.PublicKey);

            var updatedKey = await keyData.GetByPublicKey(userKey.PublicKey);
            Assert.NotNull(updatedKey);
            Assert.True(updatedKey.Roles.Count == 0);

        }
    }
}
