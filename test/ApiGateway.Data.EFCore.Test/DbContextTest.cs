using System.IO;
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
        /*
        [Fact]
        public async Task CheckDatabaseCreation()
        {
            var ctx = await GetContext();

            // Check if a new database is created. Row count should be 0
            var count = await ctx.Keys.CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task GenerateDbScript()
        {
            var ctx = await GetContext();

            var dbScript = ctx.Database.GenerateCreateScript();

            File.WriteAllText(@"D:\Tmp\createdb.sql", dbScript);
        }
        */
    }
}