using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Models;
using Xunit;

namespace ApiGateway.Data.EFCore.Test
{
    public class ApiDataTest : TestBase
    {
        [Fact]
        public async Task<ApiModel> CreateApi()
        {
            var rootKey = await GetRootKey();

            var serviceData = await GetServiceData();
            var apiData = await GetApiData();

            var serviceModel = new ServiceModel {Name = "Test service", OwnerKeyId = rootKey.Id};
            var savedService = await serviceData.Create(rootKey.PublicKey, serviceModel);

            var apiModel = new ApiModel()
            {
                Name = "Test Api",
                ServiceId = savedService.Id,
                HttpMethod = ApiHttpMethods.Get,
                Url = "http://testapi.com"
            };
            var savedApi = await apiData.Create(rootKey.PublicKey, apiModel);

            Assert.Equal(apiModel.Name, savedApi.Name);

            return savedApi;
        }

        [Fact]
        public async Task UpdateApi()
        {
            var rootKey = await GetRootKey();

            var apiData = await GetApiData();

            var existingApi = await CreateApi();

            existingApi.Name = "Updated Api Name";
            var updatedApi = await apiData.Update(rootKey.PublicKey, existingApi);

            Assert.Equal(existingApi.Name, updatedApi.Name);
        }
    }
}