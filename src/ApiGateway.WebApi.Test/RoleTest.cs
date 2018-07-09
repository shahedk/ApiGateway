using System;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Extensions;
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
            var savedService = await serviceData.Create(userKey.PublicKey, serviceModel);

            var model = new RoleModel()
            {
                OwnerKeyId = userKey.Id,
                Name = "Test Role " + DateTime.Now,
                ServiceId = savedService.Id
            };

            var savedModel = await controller.Post(model);

            Assert.Equal(model.Name, savedModel.Name);

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
    }
}