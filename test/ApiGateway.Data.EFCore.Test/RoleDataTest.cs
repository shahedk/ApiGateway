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

            var userKey = await data.Create(key);

            Assert.Equal(key.PublicKey, userKey.PublicKey);

            // # 2. Create service
            var serviceModel = new ServiceModel(){Name = "TestService", OwnerKeyId = userKey.Id};
            var savedService = await serviceData.Create(serviceModel);
            
            // # 3. Create role
            var roleModel = new RoleModel(){Name = "TestRole", OwnerKeyId = userKey.Id, ServiceId = savedService.Id};
            var savedRole = await roleData.Create(roleModel);

            // # 4. Assign role to key
            await roleData.AddKeyInRole(userKey.Id, savedRole.Id, userKey.Id);

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

            var userKey = await data.Create(key);

            Assert.Equal(key.PublicKey, userKey.PublicKey);

            // # 2. Create service
            var serviceModel = new ServiceModel(){Name = "TestService", OwnerKeyId = userKey.Id};
            var savedService = await serviceData.Create(serviceModel);
            
            // # 3. Create role
            var roleModel = new RoleModel(){Name = "TestRole", OwnerKeyId = userKey.Id, ServiceId = savedService.Id};
            var savedRole = await roleData.Create(roleModel);

            // # 4. Assign role to key
            await roleData.AddKeyInRole(userKey.Id, savedRole.Id, userKey.Id);

            var savedKey = await keyData.GetByPublicKey(userKey.PublicKey);

            Assert.NotNull(savedKey);
            Assert.True(savedKey.Roles.Count == 1);
            Assert.Equal(savedKey.Roles[0].Name , roleModel.Name);

            // # 5. Remove role
            await roleData.RemoveKeyFromRole(userKey.Id, savedRole.Id, userKey.Id);

            var updatedKey = await keyData.GetByPublicKey(userKey.PublicKey);
            Assert.NotNull(updatedKey);
            Assert.True(updatedKey.Roles.Count == 0);

        }


        [Fact]
        public async Task AddApiInRole()
        {
            var rootKey = await GetRootKey();
            var serviceData = await GetServiceData();
            var roleData = await GetRoleData();
            var apiData = await GetApiData();

            // # 1. Create service
            var serviceModel = new ServiceModel(){Name = "TestService", OwnerKeyId = rootKey.Id};
            var savedService = await serviceData.Create(serviceModel);
            
            // # 2. Create Api
            var apiModel = new ApiModel(){ Name = "Test Api", OwnerKeyId = rootKey.Id, HttpMethod = ApiHttpMethods.Get, ServiceId = savedService.Id, Url = "/testurl"};
            var savedApi = await apiData.Create(apiModel);

            // # 3. Create role
            var roleModel = new RoleModel(){Name = "TestRole", OwnerKeyId = rootKey.Id, ServiceId = savedService.Id};
            var savedRole = await roleData.Create(roleModel);

            // # 4. Assign Api to Role
            await roleData.AddApiInRole(rootKey.Id, savedRole.Id, savedApi.Id);

            var apiInfo = await apiData.Get(rootKey.Id, savedApi.Id);

            Assert.NotNull(apiInfo);
            Assert.True(apiInfo.Roles.Count == 1);
            Assert.Equal(apiInfo.Roles[0].Name , roleModel.Name);
        }

        [Fact]
        public async Task RemoveApiFromRole()
        {
            var rootKey = await GetRootKey();
            var serviceData = await GetServiceData();
            var roleData = await GetRoleData();
            var apiData = await GetApiData();

            // # 1. Create service
            var serviceModel = new ServiceModel(){Name = "TestService", OwnerKeyId = rootKey.Id};
            var savedService = await serviceData.Create(serviceModel);
            
            // # 2. Create Api
            var apiModel = new ApiModel(){ Name = "Test APi", OwnerKeyId = rootKey.Id, HttpMethod = ApiHttpMethods.Get, ServiceId = savedService.Id, Url = "/testurl"};
            var savedApi = await apiData.Create(apiModel);

            // # 3. Create role
            var roleModel = new RoleModel(){Name = "TestRole", OwnerKeyId = rootKey.Id, ServiceId = savedService.Id};
            var savedRole = await roleData.Create(roleModel);
            
            // # 4. Assign Api to Role
            await roleData.AddApiInRole(rootKey.Id, savedRole.Id, savedApi.Id);

            var apiInfo = await apiData.Get(rootKey.Id, savedApi.Id);

            Assert.NotNull(apiInfo);
            Assert.True(apiInfo.Roles.Count == 1);
            Assert.Equal(apiInfo.Roles[0].Name , roleModel.Name);

            // # 5. Remove Api from Role
            await roleData.RemoveApiFromRole(rootKey.Id, savedRole.Id, savedApi.Id);

            var updatedAPi = await apiData.Get(rootKey.Id, savedApi.Id);

            Assert.NotNull(updatedAPi);
            Assert.True(updatedAPi.Roles.Count == 0);

        }
    }
}
