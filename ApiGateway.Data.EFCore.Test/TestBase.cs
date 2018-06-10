using System.Threading.Tasks;
using ApiGateway.Data.EFCore.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiGateway.Data.EFCore.Test
{
    public class TestBase
    {
        private ApiGatewayContext _context;

        protected DbContextOptions<ApiGatewayContext> GetInMemoryOptions()
        {
            var options = new DbContextOptionsBuilder<ApiGatewayContext>()
                //.UseInMemoryDatabase(databaseName: "ApiGatewayDB").Options;
                .UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0")
                .Options;

            return options;
        }

        protected DbContextOptions<ApiGatewayContext> GetSqlServerOptions()
        {
            var options = new DbContextOptionsBuilder<ApiGatewayContext>()
                .UseSqlite("").Options;

            return options;
        }

        protected async Task<ApiGatewayContext> GetContext()
        {
            if (_context == null)
            {
                _context = new ApiGatewayContext(GetInMemoryOptions());
                var created = await _context.Database.EnsureCreatedAsync();
            }
            
            return _context;
        }
        
        protected  async Task <IKeyData> GetKeyData()
        {
            var localizer = new Mock<IStringLocalizer<KeyData>>();
            var logger = new Mock<ILogger<KeyData>>();

            var context = await GetContext();
            return new KeyData(context, localizer.Object, logger.Object);
        }
        
        protected  async Task <IServiceData> GetServiceData()
        {
            var localizer = new Mock<IStringLocalizer<ServiceData>>();
            var logger = new Mock<ILogger<ServiceData>>();
            var keyData = await GetKeyData();

            var context = await GetContext();
            return new ServiceData(context, localizer.Object, logger.Object, keyData);
        }
        
        protected  async Task <IApiData> GetApiData()
        {
            var localizer = new Mock<IStringLocalizer<ApiData>>();
            var logger = new Mock<ILogger<ApiData>>();

            var context = await GetContext();
            return new ApiData(context, localizer.Object, logger.Object);
        }

    }
}