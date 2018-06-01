using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Data.EFCore.Test
{
    public class TestBase
    {
        protected ApiGatewayContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApiGatewayContext>()
                .UseInMemoryDatabase(databaseName: "ApiGatewayDB").Options;

            var context = new ApiGatewayContext(options);

            return context;
        }
    }
}