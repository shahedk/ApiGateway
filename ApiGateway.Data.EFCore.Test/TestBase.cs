using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiGateway.Data.EFCore.Test
{
    public class TestBase
    {
        private KeyModel _ownerKeyModel = null;

        private ApiGatewayContext _context;

        protected DbContextOptions<ApiGatewayContext> GetInMemoryOptions()
        {
            var options = new DbContextOptionsBuilder<ApiGatewayContext>()
                .UseInMemoryDatabase(databaseName: "ApiGatewayDB").Options;
                

            return options;
        }

        protected DbContextOptions<ApiGatewayContext> GetSqlLocalDbOptions()
        {
            var options = new DbContextOptionsBuilder<ApiGatewayContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ApiGatewayDB;Trusted_Connection=True;ConnectRetryCount=0")
                .Options;

            return options;
        }

        protected DbContextOptions<ApiGatewayContext> GetSqliteDbOptions()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApiGatewayContext>()
                .UseSqlite(connection) // Set the connection explicitly, so it won't be closed automatically by EF
                .Options;
             

            return options;
        }

        protected async Task<ApiGatewayContext> GetContext()
        {
            if (_context == null)
            {
                _context = new ApiGatewayContext(GetSqliteDbOptions());

                // Create new database
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

        protected  async Task <IRoleData> GetRoleData()
        {
            var localizer = new Mock<IStringLocalizer<RoleData>>();
            var logger = new Mock<ILogger<RoleData>>();
            var keyData = await GetKeyData();
            var context = await GetContext();
            return new RoleData(context, localizer.Object, logger.Object, keyData);
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

        protected async Task<KeyModel> GetRootKey()
        {
            if (_ownerKeyModel == null)
            {
                var keyData = await GetKeyData();

                var keyModel = new KeyModel()
                {
                    OwnerKeyId = "0",
                    PublicKey = ModelHelper.GeneratePublicKey(),
                    Type = ApiKeyTypes.ClientSecret

                };
                _ownerKeyModel = await keyData.Create(string.Empty, keyModel);
            }

            return _ownerKeyModel;
        }

    }
}