using System;
using System.Threading.Tasks;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using Xunit;

namespace ApiGateway.Core.Test
{
    public class ServiceManagerTest : CoreTestBase
    {
        [Fact]
        public async Task<ServiceModel> CreateService()
        {
            var serviceManager = await GetServiceManager();
            var rootKey = await GetRootKey();

            // Insert new Service
            var serviceModel = new ServiceModel()
            {
                Name = "TestService " + ModelHelper.GenerateNewId(),
                OwnerKeyId = rootKey.Id
            };

            var savedService = await serviceManager.Create(rootKey.PublicKey, serviceModel);

            Assert.True(int.Parse(savedService.Id) > 0);
            Assert.True(savedService.Name == serviceModel.Name);

            return savedService;

        }

        [Fact]
        public async Task UpdateService()
        {
            var serviceManager = await GetServiceManager();
            var model = await CreateService();
            var rootKey = await GetRootKey();

            model.Name = "New Service Name " + DateTime.Now.Ticks;

            var service  = await serviceManager.Update(rootKey.PublicKey, model);

            Assert.Equal(model.Name, service.Name);
        }

        [Fact]
        public async Task DeleteService()
        {
            var serviceData = await GetServiceManager();
            var model = await CreateService();
            var rootKey = await GetRootKey();

            await serviceData.Delete(rootKey.PublicKey, model.Id);

            try
            {
                var existing = await serviceData.Get(rootKey.PublicKey, model.Id);
                Assert.Null(existing);
            }
            catch (Exception ex)
            {
                Assert.True(ex is ItemNotFoundException);
            }

        }
    }
}