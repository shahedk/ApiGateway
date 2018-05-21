using System;
using ApiGateway.Common.Extensions;
using ApiGateway.Data.EFCore.Entity;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ApiGateway.Data.EFCore.Test
{
    public class ServiceDataTest
    {
        private ApiGatewayContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApiGatewayContext>()
                                .UseInMemoryDatabase(databaseName: "ApiGatewayDB").Options;

            var context = new ApiGatewayContext(options);

            return context;
        }

        [Fact]
        public void AddNewService()
        {
            var ctx = GetContext();

            var key = new Key()
            {
                Id = ModelHelper.GenerateNewId(), 
                PublicKey = ModelHelper.GenerateNewId(),
            };

            ctx.Keys.Add(key);
            var saveKeyResult = ctx.SaveChanges();

            var service = new Service()
            {
                Id = ModelHelper.GenerateNewId(),
                Name = "TestService",
                OwnerKeyId = key.Id,
            };

            ctx.Services.Add(service);
            var saveServiceResult = ctx.SaveChanges();
        }
    }
}