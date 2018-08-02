using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Data.EFCore.Test
{
    public class TestBase
    {
        private KeyModel _rootKeyModel = null;
        private KeyModel _userKeyModel = null;
        private ServiceModel _defaultServiceModel = null;

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
            var context = await GetContext();
            return new KeyData(context);
        }

        protected  async Task <IRoleData> GetRoleData()
        {
            var context = await GetContext();
            return new RoleData(context);
        }
        
        protected  async Task <IServiceData> GetServiceData()
        {
            var context = await GetContext();
            return new ServiceData(context);
        }
        
        protected  async Task <IApiData> GetApiData()
        {
            var context = await GetContext();
            return new ApiData(context);
        }

        protected async Task<KeyModel> GetRootKey()
        {
            if (_rootKeyModel == null)
            {
                var keyData = await GetKeyData();

                var keyModel = new KeyModel()
                {
                    OwnerKeyId = "0",
                    PublicKey = ModelHelper.GeneratePublicKey(),
                    Type = ApiKeyTypes.ClientSecret

                };
                _rootKeyModel = await keyData.Create(keyModel);
            }

            return _rootKeyModel;
        }

        protected async Task<KeyModel> GetUserKey()
        {
            if (_userKeyModel == null)
            {
                var rootKey = await GetRootKey();
                var keyData = await GetKeyData();

                var keyModel = new KeyModel()
                {
                    OwnerKeyId = rootKey.Id,
                    PublicKey = ModelHelper.GeneratePublicKey(),
                    Type = ApiKeyTypes.ClientSecret

                };
                _userKeyModel = await keyData.Create(keyModel);
            }

            return _userKeyModel;
        }

        protected async Task<ServiceModel> GetServiceModel()
        {
            if (_defaultServiceModel == null)
            {
                var rootKey = await GetRootKey();
                var model = new ServiceModel() {Name = "Test service", OwnerKeyId = rootKey.Id};

                var serviceData = await GetServiceData();
                _defaultServiceModel = await serviceData.Create(model);
            }

            return _defaultServiceModel;
        }

    }
}