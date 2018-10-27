using System;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Models;
using Xunit;

namespace ApiGateway.Core.Test
{
    public class ApiManagerTest : CoreTestBase
    {
        [Fact]
        public async Task<ApiModel> CreateApi()
        {
            var rootKey = await GetRootKey();

            var serviceManager = await GetServiceManager();
            var apiManager = await GetApiManager();

            var serviceModel = new ServiceModel
            {
                Name = "Test service " + DateTime.Now, 
                OwnerKeyId = rootKey.Id
            };
            
            var savedService = await serviceManager.Create(rootKey.PublicKey, serviceModel);

            var apiModel = new ApiModel
            {
                OwnerKeyId = savedService.OwnerKeyId,
                Name = "Test Api " + DateTime.Now,
                ServiceId = savedService.Id,
                HttpMethod = ApiHttpMethods.Get,
                Url = "http://testapi.com/" + DateTime.Now + "/"
            };
            var savedApi = await apiManager.Create(rootKey.PublicKey, apiModel);

            Assert.Equal(apiModel.Name, savedApi.Name);

            return savedApi;
        }
        
        [Fact]
        public async Task UpdateApiWithUniqueName()
        {
            var rootKey = await GetRootKey();

            var apiData = await GetApiManager();

            var existingApi = await CreateApi();

            existingApi.Name = "Updated Api Name " + DateTime.Now;
            var updatedApi = await apiData.Update(rootKey.PublicKey, existingApi);

            Assert.Equal(existingApi.Name, updatedApi.Name);
        }
        
        [Fact]
        public async Task UpdateApiWithSameName()
        {
            var rootKey = await GetRootKey();

            var apiData = await GetApiManager();

            var existingApi1 = await CreateApi();
            var existingApi2 = await CreateApi();

            existingApi1.Name = existingApi2.Name;

            try
            {
                await apiData.Update(rootKey.PublicKey, existingApi1);
            }
            catch (Exception ex)
            {
                Assert.True(ex is DataValidationException);    
            }

            
            
            
        }
    }
}