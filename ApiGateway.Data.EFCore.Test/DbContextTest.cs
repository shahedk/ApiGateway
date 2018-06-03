using System.Linq;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.Extensions;
using Xunit;

namespace ApiGateway.Data.EFCore.Test
{
    public class DbContextTest : TestBase
    {
        [Fact]
        public void AddKey()
        {
            var ctx = GetContext();
            
            // Insert new Key
            var keyModel = new KeyModel()
            {
                Id = ModelHelper.GenerateNewId(), 
                PublicKey = ModelHelper.GenerateNewId(),
            };

            var keyEntity = keyModel.ToEntity();
            ctx.Keys.Add(keyEntity);
            var saveKeyResult = ctx.SaveChanges();

            Assert.Equal(1, saveKeyResult);

            // Get key from database
            var k = ctx.Keys.SingleOrDefault(x => x.Id == keyModel.Id);
            Assert.NotNull(k);
            Assert.Equal(k.Id, keyModel.Id);
        }

        [Fact]
        public void AddService()
        {
            var ctx = GetContext();
            
            // Insert new Key
            var keyModel = new KeyModel()
            {
                Id = ModelHelper.GenerateNewId(), 
                PublicKey = ModelHelper.GenerateNewId(),
            };

            var keyEntity = keyModel.ToEntity();
            ctx.Keys.Add(keyEntity);
            var saveKeyResult = ctx.SaveChanges();

            Assert.Equal(1, saveKeyResult);

            // Insert new Service
            var serviceModel = new ServiceModel()
            {
                Id = ModelHelper.GenerateNewId(),
                Name = "TestService"
            };

            var serviceEntity = serviceModel.ToEntity(keyModel.Id);
            ctx.Services.Add(serviceEntity);
            var saveServiceResult = ctx.SaveChanges();

            Assert.Equal(1, saveServiceResult);

            // Get Service from database
            var s = ctx.Services.SingleOrDefault(x => x.Id == serviceEntity.Id);
            Assert.NotNull(s);
            Assert.Equal(s.Name, serviceModel.Name);
            Assert.Equal(s.Id, serviceModel.Id);
        }


    }
}