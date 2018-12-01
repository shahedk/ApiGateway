using System;
using System.IO;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Data;
using ApiGateway.Data.EFCore;
using ApiGateway.Data.EFCore.DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Core.Test
{
    public class TestBase : IDisposable
    {
        private KeyModel _rootKeyModel;
        
        private  ApiGatewayContext _context;

        private static int _threadCounter = 1;
        private string _dbFileName;
        protected TestBase()
        {
            _dbFileName = "ApiGateway_" + _threadCounter++ + ".db";

            if (File.Exists(_dbFileName))
            {
                File.Delete(_dbFileName);
            }
            
            _context = new ApiGatewayContext(GetSqliteDbOptions());
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            File.Delete(_dbFileName);
        }

        private DbContextOptions<ApiGatewayContext> GetSqliteDbOptions()
        {
            var conStr = "DataSource=" + _dbFileName;
            var connection = new SqliteConnection(conStr);
            connection.Open();
            var options = new DbContextOptionsBuilder<ApiGatewayContext>()
                .UseSqlite(connection) // Set the connection explicitly, so it won't be closed automatically by EF
                .Options;
             

            return options;
        }

        protected  IKeyData GetKeyData()
        {
            return new KeyData(_context);
        }

        protected IRoleData GetRoleData()
        {
            return new RoleData(_context);
        }
        
        protected IServiceData GetServiceData()
        {
            return new ServiceData(_context);
        }
        
        protected IApiData GetApiData()
        {
            return new ApiData(_context);
        }

        protected async Task<KeyModel> GetRootKey()
        {
            if (_rootKeyModel == null)
            {
                var keyData = GetKeyData();

                var keyModel = new KeyModel
                {
                    OwnerKeyId = "0",
                    PublicKey = ModelHelper.GeneratePublicKey(),
                    Type = ApiKeyTypes.ClientSecret,
                    Properties = {[ApiKeyPropertyNames.ClientSecret1] = ModelHelper.GenerateSecret()}
                };


                _rootKeyModel = await keyData.Create(keyModel);
            }

            return _rootKeyModel;
        }
    }
}