using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.DataAccess;
using Microsoft.EntityFrameworkCore.Metadata;
using Xunit;

namespace ApiGateway.Data.EFCore.Test
{
    public class ServiceDataTest : TestBase
    {
        private IServiceData GetServiceData()
        {
            var ctx = GetContext();
            return new ServiceData(ctx);
        }
        
        private IKeyData GetKeyData()
        {
            var ctx = GetContext();
            return new KeyData(ctx);
        }

        [Fact]
        public void AddService()
        {
            var data = GetServiceData();
            
            // Insert new Key
            var keyModel = new KeyModel()
            {
                Id = ModelHelper.GenerateNewId(), 
                PublicKey = ModelHelper.GenerateNewId(),
            };

            // Insert new Service
            var serviceModel = new ServiceModel()
            {
                Id = ModelHelper.GenerateNewId(),
                Name = "TestService"
            };

            
            //data.SaveService()
        }
    }
}