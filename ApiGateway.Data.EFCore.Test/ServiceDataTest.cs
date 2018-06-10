using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.DataAccess;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Localization;
using Xunit;

namespace ApiGateway.Data.EFCore.Test
{
    public class ServiceDataTest : TestBase
    {
        [Fact]
        public async Task AddService()
        {
            var serviceData = await GetServiceData();
            var keyData = await GetKeyData();
            
            // Insert new Key
            var keyModel = new KeyModel()
            {
                PublicKey = ModelHelper.GenerateNewId(),
                Type = ApiKeyTypes.ClientSecret
            };
            var savedKey = await keyData.Create(string.Empty, keyModel);

            // Insert new Service
            var serviceModel = new ServiceModel()
            {
                Name = "TestService" + ModelHelper.GenerateNewId(),
                OwnerKeyId = savedKey.Id
            };

            var savedService = await serviceData.Create(savedKey.PublicKey, serviceModel);

            Assert.True(int.Parse(savedService.Id) > 0);
            Assert.True(savedService.Name == serviceModel.Name);

        }
    }
}