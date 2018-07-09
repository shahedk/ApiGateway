using System;
using System.Threading.Tasks;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.Test;
using ApiGateway.WebApi.Controllers;
using Microsoft.Extensions.Localization;
using Moq;
using Xunit;

namespace ApiGateway.WebApi.Test
{
    public class ServiceTest : WebApiTestBase
    {
        public async Task<ServiceController> GetServiceController()
        {
            var localizer = new Mock<IStringLocalizer<ServiceController>>();
            var keyData = await GetKeyData();
            var keyValidator = await GetApiKeyValidator();
            var serviceData = await GetServiceData();
            var rootKey = await GetRootKey();
            var requestHelper = new MockApiRequestHelper(rootKey.PublicKey);
            var serviceController = new ServiceController(serviceData, keyValidator, keyData, localizer.Object, requestHelper);

            return serviceController;
        }

        [Fact]
        public async Task <ServiceModel> Create()
        {
            var controller = await GetServiceController();

            var rootKey = await GetRootKey();

            var model = new ServiceModel(){ Name = "Test service", OwnerKeyId = rootKey.PublicKey};

            var savedModel = await controller.Post(model);

            Assert.Equal(model.Name, savedModel.Name);

            return savedModel;
        }

        [Fact]
        public async Task Update()
        {
            var controller = await GetServiceController();
            var existing = await Create();

            existing.Name = "Updated service name";

            var savedModel = await controller.Put(existing.Id, existing);

            Assert.Equal(existing.Name, savedModel.Name);
        }

        [Fact]
        public async Task Get()
        {
            var controller = await GetServiceController();
            var existing = await Create();

            var savedModel = await controller.Get(existing.Id);

            Assert.Equal(existing.Name, savedModel.Name);
        }

        [Fact]
        public async Task Delete()
        {
            var controller = await GetServiceController();
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
