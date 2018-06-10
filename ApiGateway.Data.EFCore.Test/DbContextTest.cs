using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ApiGateway.Data.EFCore.Test
{
    public class DbContextTest : TestBase
    {
        [Fact]
        public async Task CheckDatabaseCreation()
        {
            var ctx = await GetContext();

            // Check if a new database is created. Row count should be 0
            var count = await ctx.Keys.CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task AddKey()
        {
            var ctx = await GetContext();
            
            // Insert new Key
            var keyModel = new KeyModel()
            {
                PublicKey = ModelHelper.GenerateNewId(),
                Type = ApiKeyTypes.ClientSecret
            };

            var keyEntity = keyModel.ToEntity();
            ctx.Keys.Add(keyEntity);
            var saveKeyResult = ctx.SaveChanges();

            Assert.Equal(1, saveKeyResult);

            // Get key from database
            var savedKey = ctx.Keys.SingleOrDefault(x => x.PublicKey == keyModel.PublicKey);
            Assert.NotNull(savedKey);
            Assert.Equal(savedKey.Type, keyModel.Type);
        }

        [Fact]
        public  async Task  AddService()
        {
            var ctx = await GetContext();
            
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
            Assert.Equal(s.Id.ToString(), serviceModel.Id);
        }


    }
}