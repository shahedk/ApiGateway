using System;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Models;
using Xunit;

namespace ApiGateway.WebApi.Test
{
    public class RoleTest : WebApiTestBase
    { 
        [Fact]
        public async Task<RoleModel> Create()
        {
            var controller = await GetRoleController();

            var userKey = await GetUserKey();
            var serviceData = await GetServiceData();

             
            // # 2. Create service
            var serviceModel = new ServiceModel(){Name = "TestService", OwnerKeyId = userKey.Id};
            var savedService = await serviceData.Create(serviceModel);

            var model = new RoleModel()
            {
                OwnerKeyId = userKey.Id,
                Name = "Test Role " + DateTime.Now,
                ServiceId = savedService.Id
            };

            var savedModel = await controller.Post(model);
            
            var roleModel = await controller.Get(savedModel.Id);

            Assert.Equal(model.Name, savedModel.Name);
            Assert.Equal(model.Name, roleModel.Name);

            return savedModel;
        }

        [Fact]
        public async Task Update()
        {
            var controller = await GetRoleController();

            var model = await Create();

            model.Name = DateTime.Now.ToString();

            var savedModel = await controller.Put(model.Id, model);

            Assert.Equal(model.Name, savedModel.Name);

        }

        [Fact]
        public async Task Get()
        {
            var controller = await GetRoleController();
            var existing = await Create();

            var savedModel = await controller.Get(existing.Id);

            Assert.Equal(existing.Name, savedModel.Name);
        }

        [Fact]
        public async Task Delete()
        {
            var controller = await GetRoleController();
            var existing = await Create();

            await controller.Delete(existing.Id);

            try
            {
                var saved = await controller.Get(existing.Id);
                Assert.Null(saved);
            }
            catch (Exception ex)
            {
                Assert.True( ex is ItemNotFoundException);
            }
        }

        [Fact]
        public async Task AddApiInRole()
        {
            var role = await Create();
            var apiController = await GetApiController();
            var rootkey = await GetRootKey();
            var service = await GetServiceModel();

            var apiModel = new ApiModel(){ Name = "Test api" + DateTime.Now, HttpMethod = ApiHttpMethods.Get, Url = "https://apitest.com", ServiceId = service.Id};

            var savedApi = await apiController.Post(apiModel);

            var controller = await GetRoleController();
            await controller.AddApiInRole(savedApi.Id, role.Id);

            savedApi = await apiController.Get(savedApi.Id);

            Assert.True(savedApi.Roles.Count == 1);
            Assert.True(savedApi.Roles[0].Name == role.Name);
        }

        [Fact]
        public async Task RemoveApiFromRole()
        {
            var role = await Create();
            var apiController = await GetApiController();
            var service = await GetServiceModel();

            var apiModel = new ApiModel(){ Name = "Test api" + DateTime.Now, HttpMethod = ApiHttpMethods.Get, Url = "https://apitest.com", ServiceId = service.Id};

            var savedApi = await apiController.Post(apiModel);

            var controller = await GetRoleController();
            await controller.AddApiInRole(savedApi.Id, role.Id);

            // Check Api in roles
            savedApi = await apiController.Get(savedApi.Id);
            Assert.True(savedApi.Roles.Count == 1);
            Assert.True(savedApi.Roles[0].Name == role.Name);

            await controller.RemoveApiFromRole(savedApi.Id, role.Id);

            // Check Api in roles
            savedApi = await apiController.Get(savedApi.Id);
            Assert.True(savedApi.Roles.Count == 0);
        }

        [Fact]
        public async Task AddKeyInRole()
        {
            var role = await Create();
            var userKey = await GetUserKey();

            var roleController = await GetRoleController();
            await roleController.AddKeyInRole(userKey.PublicKey, role.Id);

            var keyRole = await GetKeyController();
            var savedKey = await keyRole.Get(userKey.Id);

            Assert.True(savedKey.Roles.Count == 1);
            Assert.True(savedKey.Roles[0].Name == role.Name);
        }

        [Fact]
        public async Task RemoveKeyFromRole()
        {
            var role = await Create();
            var userKey = await GetUserKey();

            var roleController = await GetRoleController();
            await roleController.AddKeyInRole(userKey.PublicKey, role.Id);

            var keyRole = await GetKeyController();
            var savedKey = await keyRole.Get(userKey.Id);
            
            Assert.True(savedKey.Roles.Count == 1);
            Assert.True(savedKey.Roles[0].Name == role.Name);

            await roleController.RemoveKeyFromRole(userKey.PublicKey, role.Id);
            savedKey = await keyRole.Get(userKey.Id);
            
            Assert.True(savedKey.Roles.Count == 0);
        }
    }
}