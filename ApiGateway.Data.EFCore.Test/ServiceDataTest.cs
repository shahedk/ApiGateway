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
            var ownerKey = await GetOwnerKey();

            // Insert new Service
            var serviceModel = new ServiceModel()
            {
                Name = "TestService" + ModelHelper.GenerateNewId(),
                OwnerKeyId = ownerKey.Id
            };

            var savedService = await serviceData.Create(ownerKey.PublicKey, serviceModel);

            Assert.True(int.Parse(savedService.Id) > 0);
            Assert.True(savedService.Name == serviceModel.Name);

            return savedService;

        }

        [Fact]
        public async Task UpdateService()
        {
            var serviceData = await GetServiceData();
            var model = await CreateService();
            var ownerKey = await GetOwnerKey();

            model.Name = "some new name";

            var updatedServie  = await serviceData.Update(ownerKey.PublicKey, model);

            Assert.Equal(model.Name, updatedServie.Name);
        }

        [Fact]
        public async Task DeleteService()
        {
            var serviceData = await GetServiceData();
            var model = await CreateService();
            var ownerKey = await GetOwnerKey();

            await serviceData.Delete(ownerKey.PublicKey, model.Id);

            try
            {
                var existing = await serviceData.Get(ownerKey.PublicKey, model.Id);
                Assert.Null(existing);
            }
            catch (Exception ex)
            {
                Assert.True(ex is ItemNotFoundException);
            }

        }
    }
}