using System;
using System.Threading.Tasks;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using Xunit;

namespace ApiGateway.Data.EFCore.Test
{
    public class ServiceDataTest : TestBase
    {
        [Fact]
        public async Task<ServiceModel> CreateService()
        {
            var serviceData = await GetServiceData();
            var ownerKey = await GetRootKey();

            // Insert new Service
            var serviceModel = new ServiceModel()
            {
                Name = "TestService" + ModelHelper.GenerateNewId(),
                OwnerKeyId = ownerKey.Id
            };

            var savedService = await serviceData.Create(serviceModel);

            Assert.True(int.Parse(savedService.Id) > 0);
            Assert.True(savedService.Name == serviceModel.Name);

            return savedService;

        }

        [Fact]
        public async Task UpdateService()
        {
            var serviceData = await GetServiceData();
            var model = await CreateService();
            var ownerKey = await GetRootKey();

            model.Name = "some new name";

            var updatedServie  = await serviceData.Update(model);

            Assert.Equal(model.Name, updatedServie.Name);
        }

        [Fact]
        public async Task DeleteService()
        {
            var serviceData = await GetServiceData();
            var model = await CreateService();
            var ownerKey = await GetRootKey();

            await serviceData.Delete(ownerKey.Id, model.Id);

            try
            {
                var existing = await serviceData.Get(ownerKey.Id, model.Id);
                Assert.Null(existing);
            }
            catch (Exception ex)
            {
                Assert.True(ex is ItemNotFoundException);
            }

        }
    }
}